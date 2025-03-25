using System;
using System.IO;
using UnityEngine;
using ProtoBuf;
using LS = LocalizationSupports;

[System.Serializable]
[ProtoContract]
public struct SpecialCustomerSpriteDescriptor
{
    [ProtoMember(1)]
    public string frontSpritePath;
    [ProtoMember(2)]
    public string backSpritePath;
    public SpecialCustomerSpriteDescriptor(string frontSpritePath = "", string backSpritePath = "")
    {
        this.frontSpritePath = frontSpritePath;
        this.backSpritePath = backSpritePath;
    }
}

public struct SpecialCustomerSprite
{
    public Texture2D frontSprite;
    public Texture2D backSprite;
    public SpecialCustomerSprite(Texture2D frontSprite, Texture2D backSprite)
    {
        this.frontSprite = frontSprite;
        this.backSprite = backSprite;
    }
}

[System.Serializable]
[ProtoContract]
public class SpecialCustomerModel
{
    private const string _T = LocalizationTableKeys.ITEMS_TABLE;
    [ProtoMember(1)]
    public string identifier;
    [ProtoMember(2)]
    public SpecialCustomerSpriteDescriptor spriteDescriptor;

    public string DisplayName
    {
        get
        {
            return LS.__(_T, $"cafe.customer.{this.identifier}.displayname");
        }
    }

    [SerializeField]
    private SpecialCustomerSprite _sprite;
    public SpecialCustomerSprite Sprite
    {
        get
        {
            return this._sprite;
        }
    }

    public SpecialCustomerModel
    (
        string identifier,
        SpecialCustomerSpriteDescriptor spriteDescriptor
    )
    {
        this.identifier = identifier;
        this.spriteDescriptor = spriteDescriptor;

        this.InitRequirements();
    }

    private void InitRequirements()
    {
        LoadSprite2D();
    }

    private void LoadSprite2D()
    {
        Texture2D _front, _back;
        if (string.IsNullOrEmpty(this.spriteDescriptor.frontSpritePath))
        {
            throw new InvalidOperationException("this.spriteDescriptor.frontSpritePath is null");
        }
        _front = StreamingAssetsLoader.GetTexture2D(this.spriteDescriptor.frontSpritePath);

        if (string.IsNullOrEmpty(this.spriteDescriptor.backSpritePath))
        {
            throw new InvalidOperationException("this.spriteDescriptor.backSpritePath is null");
        }
        _back = StreamingAssetsLoader.GetTexture2D(this.spriteDescriptor.backSpritePath);
        this._sprite = new SpecialCustomerSprite(_front, _back);
    }
}
