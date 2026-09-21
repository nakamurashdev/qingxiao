using Terraria.Audio;

namespace Qingxiao.Content;

public sealed class WindofTranscendenceItem : ModItem
{
    /// <inheritdoc/> 
    public override string Texture => Assets.Images.Content.WindofTranscendenceItem.KEY;

    public override void Load()
    {
        base.Load();
        
        if (Main.netMode == NetmodeID.Server)
        {
            return;
        }

        EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Head}", EquipType.Head, this);
        EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Body}", EquipType.Body, this);
        EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Legs}", EquipType.Legs, this);
    }

    /// <inheritdoc/> 
    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        if (Main.netMode == NetmodeID.Server)
        {
            return;
        }

        var head = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
        var body = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
        var legs = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);

        ArmorIDs.Head.Sets.DrawHead[head] = false;
        
        ArmorIDs.Body.Sets.HidesTopSkin[body] = true;
        ArmorIDs.Body.Sets.HidesBottomSkin[body] = true;
        
        ArmorIDs.Legs.Sets.HidesTopSkin[legs] = true;
        ArmorIDs.Legs.Sets.HidesBottomSkin[legs] = true;
    }

    /// <inheritdoc/> 
    public override void SetDefaults()
    {
        base.SetDefaults();
        
        Item.hasVanityEffects = true;
        Item.accessory = true;

        Item.rare = ItemRarityID.Cyan;
    }
    
    /// <inheritdoc/> 
    public override void UpdateVanity(Player player)
    {
        base.UpdateVanity(player);
        
        player.GetModPlayer<WindofTranscendencePlayer>().Enabled = true;
    }
    
    /// <inheritdoc/> 
    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        base.UpdateAccessory(player, hideVisual);
        
        player.GetModPlayer<WindofTranscendencePlayer>().Enabled = !hideVisual;
    }

    /// <inheritdoc/> 
    public override bool AllowPrefix(int pre) => false;
}

public sealed class WindofTranscendencePlayer : ModPlayer
{
    /// <summary>
    ///     Gets or sets a value indicating whether the effects of the Wind of Transcendence are enabled.
    /// </summary>
    /// <value>
    ///     <see langword="true"/> if the effects of the Wind of Transcendence are enabled; otherwise, <see langword="false"/>.
    /// </value>
    public bool Enabled { get; set; }

    /// <inheritdoc/> 
    public override void ResetEffects()
    {
        base.ResetEffects();
        
        Enabled = false;
    }

    /// <inheritdoc/> 
    public override void FrameEffects()
    {
        base.FrameEffects();
        
        if (!Enabled)
        {
            return;
        }
        
        var item = ModContent.GetInstance<WindofTranscendenceItem>();
        
        Player.head = EquipLoader.GetEquipSlot(Mod, item.Name, EquipType.Head);
        Player.body = EquipLoader.GetEquipSlot(Mod, item.Name, EquipType.Body);
        Player.legs = EquipLoader.GetEquipSlot(Mod, item.Name, EquipType.Legs);
    }

    /// <inheritdoc/> 
    public override void ModifyHurt(ref Player.HurtModifiers modifiers)
    {
        base.ModifyHurt(ref modifiers);

        if (!Enabled)
        {
            return;
        }
        
        modifiers.DisableSound();
    }

    /// <inheritdoc/> 
    public override void OnHurt(Player.HurtInfo info)
    {
        base.OnHurt(info);
        
        if (!Enabled)
        {
            return;
        }
        
        SoundEngine.PlaySound(in SoundID.FemaleHit, Player.Center);
    }
}