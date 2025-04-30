public class AnimalScrollViewUIRefresher : UIRefresher
{
    protected override void Awake()
    {
        base.Awake();

        TestCode.OnAddAnimalInfoPanel += StartRefresh;
        TestCode.OnRemoveAnimalInfoPanel += StartRefreshAfterFrame;
    }

    private void OnDestroy()
    {
        TestCode.OnAddAnimalInfoPanel -= StartRefresh;
        TestCode.OnRemoveAnimalInfoPanel -= StartRefreshAfterFrame;
    }
}