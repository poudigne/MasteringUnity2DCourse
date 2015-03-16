using UnityEngine;
using System.Collections;

public class ConversationManager : Singleton<ConversationManager>
{

  public int _displayTextureOffset = 70;
  
  private bool _isTalking = false;
  private ConversationEntry _currentConversationLine;
  private int _fontSpacing = 7;
  private int _conversationTextWidth;
  private int _dialogHeight = 70;
  private int _delayBetweenLine = 3;
  private Rect _scaledTextureRect;
  
  protected ConversationManager() { }

  public void StartConversation(Conversation conversation)
  {
    if (!_isTalking)
    {
      StartCoroutine(DisplayConversation(conversation));
    }
  }

  IEnumerator DisplayConversation(Conversation conversation)
  {
    _isTalking = true;

    foreach (var conversationLine in conversation.conversationLines)
    {
      _currentConversationLine = conversationLine;
      _conversationTextWidth = _currentConversationLine.conversionText.Length* _fontSpacing;
      var displayPic = _currentConversationLine.displayPic;
      _scaledTextureRect = new Rect(displayPic.textureRect.x / displayPic.texture.width,
        displayPic.textureRect.y / displayPic.texture.height,
        displayPic.textureRect.width / displayPic.texture.width,
        displayPic.textureRect.height / displayPic.texture.height
        );
      yield return new WaitForSeconds(_delayBetweenLine);
    }
    _isTalking = false;
    yield return null;
  }

  void OnGUI()
  {
    if (_isTalking)
    {
      GUI.BeginGroup(new Rect(Screen.width / 2 - _conversationTextWidth /2, 50, _conversationTextWidth + _displayTextureOffset + 10, _dialogHeight));
      GUI.Box(new Rect(0, 0, _conversationTextWidth + _displayTextureOffset + 10, _dialogHeight), "");
      GUI.Label(new Rect(_displayTextureOffset, 10, _conversationTextWidth + 30, 2), _currentConversationLine.speakingCharacterName);
      GUI.Label(new Rect(_displayTextureOffset, 30, _conversationTextWidth + 30, 20), _currentConversationLine.conversionText);
      GUI.DrawTextureWithTexCoords(new Rect(10, 10, 50, 50), _currentConversationLine.displayPic.texture, _scaledTextureRect);
      GUI.EndGroup();
    }
  }

}
