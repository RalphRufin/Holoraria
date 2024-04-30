using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;


namespace Holoraria.Items
{
	public class AmeBullet: ModItem
	{
		public override void SetDefaults() 
		{
			Item.damage = 5;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 40;
			Item.height = 40;
			Item.knockBack = 2;
			Item.value = 50;
			Item.rare = 1;
			Item.consumable = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.AmeBulletProjectile>();
			Item.ammo = AmmoID.Bullet;
			Item.maxStack = 999;
			Item.shootSpeed = 10f;
			
		}

		public override void AddRecipes() 
		{
			Recipe recipe = CreateRecipe(25);
			recipe.AddIngredient(ItemID.Gel, 2);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}