namespace ClsOom.ClassOOM.model;

public interface IStreamComponent
{
    void OnSerialize(PacketStream stream);
    void OnDeSerialize(PacketStream stream);
}