using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Submission_Tracker.Model;

namespace Submission_Tracker.ViewModel
{
    /// <summary>
    /// An Implementation IResultRowPublisher of type ResultItem.
    /// This class invokes the Generator logic from top layer and helps publishing results
    /// in publisher-subscriber behavior.
    /// </summary>
    class ResultIterator : IResultRowPublisher<ResultItem>
    {
        private int _length;
        private Dictionary<string, Dictionary<string, FileCluster>> rightHash;
        private Dictionary<string, Dictionary<string, FileCluster>> leftHash;
        private int count;
        private Filters _filters = new Filters();

        public event EventHandler<RowResultSender<ResultItem>> RowPublisherHandler;

        public ResultIterator(int length)
        {
            _length = length;
        }

        public ResultIterator(Dictionary<string, Dictionary<string, FileCluster>> leftHash, Dictionary<string, Dictionary<string, FileCluster>> rightHash, int count, Filters filters)
        {
            this.rightHash = rightHash;
            this.leftHash = leftHash;
            this.count = count;
            this._filters = filters;
        }

        public ResultIterator()
        {
        }

        /// <summary>
        /// Async method to compare and calculate data from source and destination and generate
        /// row items.
        /// </summary>
        public async void GenerateAndPublishRow()
        {
            Task<ResultRow> rowBuilderTask;
            ResultRowBuilder rowBuilder = new ResultRowBuilder();
            ResultRow row = null;
            if(leftHash == null)
                return;
            foreach (var key in leftHash.Keys)
            {
                Dictionary<string, FileCluster> rightData = null;
                if (rightHash!=null && rightHash.ContainsKey(key))
                    rightData = rightHash[key];


                if (rightData != null)
                    rowBuilderTask = Task.Factory.StartNew(() => rowBuilder.GenerateRow(leftHash[key], rightHash[key], _filters));
                else
                    rowBuilderTask = Task.Factory.StartNew(() => rowBuilder.GenerateRow(leftHash[key], null, _filters));
                var result = rowBuilderTask.Result;
                if (result == null)
                    continue;
                string comment = result.Comment.SourceVersion + " " + result.Comment.CommentState + " ";
                comment += result.Comment.DestinationVersion != null ? (Int32.Parse(result.Comment.DestinationVersion) > 0 ? result.Comment.DestinationVersion : "") : "";

                await Task.WhenAll(rowBuilderTask);
                PublishRowItem(new ResultItem(result.SourceResult.Filename, result.SourceResult.FilePath, result.SourceResult.CurrentVersion,
                                                        result.SourceResult.Status.ToString(), "", result.DestinationResult.Filename, result.DestinationResult.FilePath,
                                                        result.DestinationResult.CurrentVersion, result.DestinationResult.Status.ToString(), comment));
            }
        }

        #region old code
        //public ResultItem IterateAndCreateResults(ResultIterator iterator)
        // {
        //     foreach (var result in iterator)
        //     {
        //         if (result == null)
        //             continue;
        //         string comment = result.Comment.SourceVersion + " " + result.Comment.CommentState + " ";
        //         comment += result.Comment.DestinationVersion != null ? (Int32.Parse(result.Comment.DestinationVersion) > 0 ? result.Comment.DestinationVersion : "") : "";
        //
        //         PublishRowItem(new ResultItem(result.SourceResult.Filename, result.SourceResult.FilePath, result.SourceResult.CurrentVersion,
        //                                                 result.SourceResult.Status.ToString(), "", result.DestinationResult.Filename, result.DestinationResult.FilePath,
        //                                                 result.DestinationResult.CurrentVersion, result.DestinationResult.Status.ToString(), comment));
        //         //Task dataDisplay = Task.Factory.StartNew(() => AddDisplayRowItem(result, comment));
        //         //DataGridTable.Items.Add(new ResultItem(result.SourceResult.Filename, result.SourceResult.FilePath, result.SourceResult.CurrentVersion,
        //         //                                        result.SourceResult.Status.ToString(), "", result.DestinationResult.Filename, result.DestinationResult.FilePath,
        //         //                                        result.DestinationResult.CurrentVersion, result.DestinationResult.Status.ToString(), comment));
        //     }
        //     return null;
        // }
        #endregion

        private void OnRowPublish(RowResultSender<ResultItem> args)
        {
            var handler = RowPublisherHandler;
            if (handler != null)
                handler(this, args);
        }

        public void PublishRowItem(ResultItem data)
        {
            RowResultSender<ResultItem> row = (RowResultSender<ResultItem>)Activator.CreateInstance(typeof(RowResultSender<ResultItem>), new object[] { data });
            OnRowPublish(row);
        }
    }
}
