using Service;

namespace StructureTweaksPlugin;

public class Destroy
{
  static readonly int Hash = "override_destroy".GetStableHashCode();

  public static void Handle(ZNetView view)
  {
    Helper.Float(view, Hash, value =>
    {
      var td = Helper.Get<TimedDestruction>(view);
      td.m_triggerOnAwake = true;
      td.m_timeout = value;
      td.CancelInvoke("DestroyNow");
      td.Trigger();
    });

  }
}
