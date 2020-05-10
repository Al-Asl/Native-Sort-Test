
using System.Collections.Generic;

public class MainSortRunner 
{
    private static MainSortRunner instance;

    public static MainSortRunner Instance
    {
        get
        {
            if (instance == null)
                instance = new MainSortRunner();
            return instance;
        }
    }

    private List<ILogger> loggers = new List<ILogger>();
    private BaseSortRunner managedRunner;
    private BaseSortRunner nativeRunner;

    private MainSortRunner()
    {
        managedRunner = new ManagedSortRunner();
        nativeRunner = new NativeSortRunner();
    }

    public void AddLogger(ILogger logger)
    {
        loggers.Add(logger);
    }

    public void RunSort(SortSettings settings)
    {
        Log(settings.sortType == SortType.Managed ? managedRunner.Run(settings) : nativeRunner.Run(settings) );
    }

    private void Log (string message)
    {
        for (int i = 0; i < loggers.Count; i++)
        {
            loggers[i].Log(message);
        }
    }

    public void RunCustomSort(SortSettings settings)
    {
        Log(CustomSortSource.Instance.GetSort(settings.name)(settings));
    }
}