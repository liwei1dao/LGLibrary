using System;

namespace LG.Combat;



[EnableUpdate]
public class UpdateComponent : Component
{
    public override bool DefaultEnable { get; set; } = true;

    public override void LGUpdate(float time)
    {
        Entity.LGUpdate(time);
    }
}
