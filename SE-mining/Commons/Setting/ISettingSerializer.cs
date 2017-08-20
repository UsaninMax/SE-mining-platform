namespace SEMining.Commons.Setting
{
    public interface ISettingSerializer
    {
        void Serialize<T>(T serObject, string path);
        T Deserialize<T>(string path);
    }
}
