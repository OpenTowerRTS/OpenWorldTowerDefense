// This is probably a smelly design but to make thing simple, instead of having another DeselectCommand,
//we can just use SelectCommand with null targetEntityID to represent deselection
public struct SelectCommand
{
    public EntityID? targetEntityID;
}
