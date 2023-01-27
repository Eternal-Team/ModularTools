// using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Graphics;
// using ModularTools.Content.Items.Armor;
// using ReLogic.Content;
// using Terraria;
// using Terraria.DataStructures;
// using Terraria.ModLoader;
//
// namespace ModularTools.Content;
//
// internal class DrawDataInfo
// {
// 	public Vector2 Position;
// 	public Rectangle? Frame;
// 	public float Rotation;
// 	public Texture2D Texture;
// 	public Vector2 Origin;
// }
//
// internal abstract class ModularArmorDrawLayer : PlayerDrawLayer
// {
// 	protected abstract DrawDataInfo GetData(PlayerDrawSet info);
//
// 	public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)=> drawInfo.shadow == 0f && !drawInfo.drawPlayer.invis && ShouldGlow(drawInfo);
//
// 	public abstract bool ShouldGlow(PlayerDrawSet info);
//
// 	protected override void Draw(ref PlayerDrawSet drawInfo)
// 	{
// 		var drawDataInfo = GetData(drawInfo);
// 		var drawPlayer = drawInfo.drawPlayer;
// 		var effects = SpriteEffects.None;
//
// 		if (drawPlayer.direction == -1) effects |= SpriteEffects.FlipHorizontally;
// 		if (drawPlayer.gravDir == -1) effects |= SpriteEffects.FlipVertically;
//
// 		var data = new DrawData(
// 			drawDataInfo.Texture,
// 			drawDataInfo.Position,
// 			drawDataInfo.Frame,
// 			Color.White * Main.essScale,
// 			drawDataInfo.Rotation,
// 			drawDataInfo.Origin,
// 			1f,
// 			effects,
// 			0
// 		);
//
// 		drawInfo.DrawDataCache.Add(data);
// 	}
// }
//
// internal class ChestplateLayer : ModularArmorDrawLayer
// {
// 	private static Asset<Texture2D> glowTextureMale;
// 	private static Asset<Texture2D> glowTextureFemale;
//
// 	protected override DrawDataInfo GetData(PlayerDrawSet info)
// 	{
// 		glowTextureMale ??= ModContent.Request<Texture2D>(ModularTools.TexturePath + "Armor/ModularArmor_Glow_Male");
// 		glowTextureFemale ??= ModContent.Request<Texture2D>(ModularTools.TexturePath + "Armor/ModularArmor_Glow_Female");
//
// 		return GetBodyDrawDataInfo(info, info.drawPlayer.Male ? glowTextureMale.Value : glowTextureFemale.Value);
// 	}
//
// 	public override bool ShouldGlow(PlayerDrawSet info)
// 	{
// 		Item item = info.drawPlayer.armor[1];
// 		return !item.IsAir && item.ModItem is ModularChestplate chestplate && chestplate.EnergyStorage.Energy > 0;
// 	}
//
// 	public static DrawDataInfo GetBodyDrawDataInfo(PlayerDrawSet drawInfo, Texture2D texture)
// 	{
// 		Player drawPlayer = drawInfo.drawPlayer;
// 		Vector2 pos = drawPlayer.bodyPosition + drawInfo.bodyVect + new Vector2(
// 			(int)(drawInfo.Position.X - Main.screenPosition.X - drawPlayer.bodyFrame.Width / 2f + drawPlayer.width / 2f),
// 			(int)(drawInfo.Position.Y - Main.screenPosition.Y + drawPlayer.height - drawPlayer.bodyFrame.Height + 4f)
// 		);
//
// 		return new DrawDataInfo
// 		{
// 			Position = pos,
// 			Frame = drawPlayer.bodyFrame,
// 			Origin = drawInfo.bodyVect,
// 			Rotation = drawPlayer.bodyRotation,
// 			Texture = texture
// 		};
// 	}
//
// 	public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Torso);
// }
//
// internal class HelmetLayer : ModularArmorDrawLayer
// {
// 	private static Asset<Texture2D> glowTexture;
//
// 	public override bool IsHeadLayer => true;
//
// 	protected override DrawDataInfo GetData(PlayerDrawSet info)
// 	{
// 		glowTexture ??= ModContent.Request<Texture2D>(ModularTools.TexturePath + "Armor/ModularArmor_Head_Glow");
//
// 		return GetHeadDrawDataInfo(info, glowTexture.Value);
// 	}
//
// 	public override bool ShouldGlow(PlayerDrawSet info)
// 	{
// 		Item item = info.drawPlayer.armor[0];
// 		return !item.IsAir && item.ModItem is ModularHelmet helmet && helmet.EnergyStorage.Energy > 0;
// 	}
//
// 	public static DrawDataInfo GetHeadDrawDataInfo(PlayerDrawSet drawInfo, Texture2D texture)
// 	{
// 		Player drawPlayer = drawInfo.drawPlayer;
// 		Vector2 pos = drawPlayer.headPosition + drawInfo.headVect + new Vector2(
// 			(int)(drawInfo.Position.X + drawPlayer.width / 2f - drawPlayer.bodyFrame.Width / 2f - Main.screenPosition.X),
// 			(int)(drawInfo.Position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height + 4f - Main.screenPosition.Y)
// 		);
//
// 		return new DrawDataInfo
// 		{
// 			Position = pos,
// 			Frame = drawPlayer.bodyFrame,
// 			Origin = drawInfo.headVect,
// 			Rotation = drawPlayer.headRotation,
// 			Texture = texture
// 		};
// 	}
//
// 	public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);
// }
//
// internal class LeggingsLayer : ModularArmorDrawLayer
// {
// 	private static Asset<Texture2D> glowTexture;
//
// 	protected override DrawDataInfo GetData(PlayerDrawSet info)
// 	{
// 		glowTexture ??= ModContent.Request<Texture2D>(ModularTools.TexturePath + "Armor/ModularArmor_Legs_Glow");
//
// 		return GetLegDrawDataInfo(info, glowTexture.Value);
// 	}
//
// 	public override bool ShouldGlow(PlayerDrawSet info)
// 	{
// 		Item item = info.drawPlayer.armor[2];
// 		return !item.IsAir && item.ModItem is ModularLeggings leggings && leggings.EnergyStorage.Energy > 0;
// 	}
//
// 	public static DrawDataInfo GetLegDrawDataInfo(PlayerDrawSet drawInfo, Texture2D texture)
// 	{
// 		Player drawPlayer = drawInfo.drawPlayer;
// 		Vector2 pos = drawPlayer.legPosition + drawInfo.legVect + new Vector2(
// 			(int)(drawInfo.Position.X - Main.screenPosition.X - drawPlayer.legFrame.Width / 2f + drawPlayer.width / 2f),
// 			(int)(drawInfo.Position.Y - Main.screenPosition.Y + drawPlayer.height - drawPlayer.legFrame.Height + 4f)
// 		);
//
// 		return new DrawDataInfo
// 		{
// 			Position = pos,
// 			Frame = drawPlayer.legFrame,
// 			Origin = drawInfo.legVect,
// 			Rotation = drawPlayer.legRotation,
// 			Texture = texture
// 		};
// 	}
//
// 	public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Leggings);
// }