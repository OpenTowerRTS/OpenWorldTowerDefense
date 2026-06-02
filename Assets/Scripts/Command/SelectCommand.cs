// This is probably a smelly design but to make thing simple, instead of having another DeselectCommand,
//we can just use SelectCommand with null targetEntityID to represent deselection
public readonly struct SelectCommand
{
    public EntityID? TargetEntityID { get; }

    public SelectCommand(EntityID? targetEntityID) => TargetEntityID = targetEntityID;
}
