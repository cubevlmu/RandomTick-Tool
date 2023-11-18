namespace ClsOom.ClassOOM.config;

public interface ICustomSerializable<T>
{
    string SerializeTo(T v);
    T SerializeFrom(string s);
}