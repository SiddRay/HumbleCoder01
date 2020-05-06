namespace Submission_Tracker.Model
{
    /// <summary>
    /// This enum defines the 1:1 mapping for the Filters in the main window.
    /// All files filter has AllFiles enum value.
    /// Only Integrated Files filter has OnlyIntegratedFiles enum value.
    /// Only Non Integrated Files has OnlyNonIntegratedFiles enum value.
    /// 
    /// Currently tool supports only All Files filter as true.
    /// </summary>
    public enum FilesToDisplay
    {
        AllFiles = 1,
        OnlyIntegratedFiles = 2,
        OnlyNonIntegratedFiles = 3
    }
}
