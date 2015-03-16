using UnityEngine;

public class MessagingClientReceiver : MonoBehaviour
{
  void Start()
  {
    MessagingManager.Instance.Subscribe(ThePlayerIsTryingToLeave);
  }

  void ThePlayerIsTryingToLeave()
  {
    var dialog = GetComponent<ConversationComponent>();
    if (dialog != null)
    {
      if (dialog.conversations != null && dialog.conversations.Length > 0)
      {
        var conversation = dialog.conversations[0];
        if (conversation != null)
        {
          ConversationManager.Instance.StartConversation(conversation);
        }
      }
    }
  }

  void OnDestroy()
  {
    if (MessagingManager.Instance != null)
    {
      MessagingManager.Instance.UnSubscribe(ThePlayerIsTryingToLeave);
    }
  }
}
