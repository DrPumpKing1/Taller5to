public interface IDataService
{
    bool SaveData<T>(T data);

    T LoadData<T>();
}
