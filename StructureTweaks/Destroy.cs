using Service;

namespace StructureTweaksPlugin;

public class Destroy
{
  public static void Handle(ZNetView view)
  {
    Helper.Float(view, Hash.Destroy, value =>
    {
      var td = Helper.Get<TimedDestruction>(view);
      td.m_triggerOnAwake = true;
      td.m_timeout = value;
      td.CancelInvoke("DestroyNow");
      td.Trigger();
    });

  }
}
