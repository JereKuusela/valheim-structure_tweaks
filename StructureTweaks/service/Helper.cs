using System;
namespace Service;
public class Helper {
  public static void AddMessage(Terminal context, string message, bool priority = true) {
    context.AddString(message);
    var hud = MessageHud.instance;
    if (!hud) return;
    if (priority) {
      var items = hud.m_msgQeue.ToArray();
      hud.m_msgQeue.Clear();
      Player.m_localPlayer?.Message(MessageHud.MessageType.TopLeft, message);
      foreach (var item in items)
        hud.m_msgQeue.Enqueue(item);
      hud.m_msgQueueTimer = 10f;
    } else {
      Player.m_localPlayer?.Message(MessageHud.MessageType.TopLeft, message);
    }
  }
  public static ZNetView GetHover() {
    var hovered = Player.m_localPlayer?.m_hovering;
    if (!hovered || hovered == null) throw new InvalidOperationException("Not hovering anything.");
    var piece = hovered.GetComponent<Piece>();
    if (!piece) throw new InvalidOperationException("Not hovering anything.");
    var view = piece.m_nview;
    if (!view) throw new InvalidOperationException("Not hovering anything.");
    var id = Game.instance.GetPlayerProfile().GetPlayerID();
    if (!Console.instance.IsCheatsEnabled() && view.GetZDO().GetLong(Piece.m_creatorHash, 0L) != id)
      throw new InvalidOperationException("Works only for your own structures.");
    return view;
  }
  public static void Command(string name, string description, Terminal.ConsoleEvent action, Terminal.ConsoleOptionsFetcher? fetcher = null) {
    new Terminal.ConsoleCommand(name, description, Helper.Catch(action), optionsFetcher: fetcher);
  }
  public static void AddError(Terminal context, string message, bool priority = true) {
    AddMessage(context, $"Error: {message}", priority);
  }
  public static Terminal.ConsoleEvent Catch(Terminal.ConsoleEvent action) =>
    (args) => {
      try {
        if (!Player.m_localPlayer) throw new InvalidOperationException("Player not found.");
        action(args);
      } catch (InvalidOperationException e) {
        Helper.AddError(args.Context, e.Message);
      }
    };
}