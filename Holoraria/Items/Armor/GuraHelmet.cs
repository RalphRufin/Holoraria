using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Holoraria.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class GuraHelmet : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
			Item.height = 20;
			Item.value = 1500;
			Item.rare = 1;
			Item.defense = 9;
        }

		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
			return body.type == ModContent.ItemType<GuraChestplate>() && legs.type == ModContent.ItemType<GuraLeggings>();
        }

		public override void UpdateArmorSet(Player player)
        {
			player.setBonus = "King of the Sea";
			player.AddBuff(BuffID.Flipper, 1);
			player.AddBuff(BuffID.Gills, 1);
			if (player.wet){
				player.swimTime = 1000000;
				player.moveSpeed += 0.50f;
			}
			
        }

		 public override void UpdateEquip(Player player)
        {
			player.GetCritChance(DamageClass.Melee) += 10;
        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DirtBlock, 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

		}
    }
