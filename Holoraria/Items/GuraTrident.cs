using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Holoraria.Items
{
    public class GuraTrident : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 36;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = 3;
            Item.knockBack = 10;
            Item.value = 10000;
            Item.rare = 2;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }

        public override void UpdateInventory(Player player)
        {
            // Check if the item is equipped
            if (player.HeldItem == Item)
            {
                // Modify player's stats internally
					ModifyStats(player);
				player.GetModPlayer<ExampleDashPlayer>().TridentEquipped = true;
            }
            else
            {
                // Reset player's stats when not equipped
                ResetStats(player);
            }
        }

        private void ModifyStats(Player player)
        {
            // Modify melee damage and crit chance using player.GetDamage
            float damageModifier = 1.3f; // +20% melee damage

            player.GetDamage(DamageClass.Melee) = player.GetDamage(DamageClass.Melee) * damageModifier;
        }

        private void ResetStats(Player player)
        {
            // Reset player's stats when not equipped
            float damageModifier = 0.7f;

            player.GetDamage(DamageClass.Melee) = player.GetDamage(DamageClass.Melee) * damageModifier;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DirtBlock, 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

		public class ExampleDashPlayer : ModPlayer
	{
		// These indicate what direction is what in the timer arrays used
		
		public const int DashDown = 0;
		public const int DashUp = 1;
		public const int DashRight = 2;
		public const int DashLeft = 3;

		public const int DashCooldown = 60; // Time (frames) between starting dashes. If this is shorter than DashDuration you can start a new dash before an old one has finished
		public const int DashDuration = 35; // Duration of the dash afterimage effect in frames

		// The initial velocity.  10 velocity is about 37.5 tiles/second or 50 mph
		public const float DashVelocity = 15f;

		// The direction the player has double tapped.  Defaults to -1 for no dash double tap
		public int DashDir = -1;

		// The fields related to the dash accessory
		public bool TridentEquipped;
		public int DashDelay = 0; // frames remaining till we can dash again
		public int DashTimer = 0; // frames remaining in the dash


		public override void ResetEffects() {
			// Reset our equipped flag. If the accessory is equipped somewhere, ExampleShield.UpdateAccessory will be called and set the flag before PreUpdateMovement
			TridentEquipped = false;

			// ResetEffects is called not long after player.doubleTapCardinalTimer's values have been set
			// When a directional key is pressed and released, vanilla starts a 15 tick (1/4 second) timer during which a second press activates a dash
			// If the timers are set to 15, then this is the first press just processed by the vanilla logic.  Otherwise, it's a double-tap
			if (Player.controlDown && Player.releaseDown && Player.doubleTapCardinalTimer[DashDown] < 15) {
				DashDir = DashDown;
			
			}
			else if (Player.controlUp && Player.releaseUp && Player.doubleTapCardinalTimer[DashUp] < 15) {
				DashDir = DashUp;
			
			}
			else if (Player.controlRight && Player.releaseRight && Player.doubleTapCardinalTimer[DashRight] < 15) {
				DashDir = DashRight;
		
			}
			else if (Player.controlLeft && Player.releaseLeft && Player.doubleTapCardinalTimer[DashLeft] < 15) {
				DashDir = DashLeft;
	
			}
			else {
				DashDir = -1;
			}
		}
		private int dashDelayTimer;
		private int buffDelayTimer;
		 private int timer = 120;
    	private int featherfallTimer = 0;
		// This is the perfect place to apply dash movement, it's after the vanilla movement code, and before the player's position is modified based on velocity.
		// If they double tapped this frame, they'll move fast this frame
		
		public override void PreUpdateMovement() {
			// if the player can use our dash, has double tapped in a direction, and our dash isn't currently on cooldown
			if (CanUseDash() && DashDir != -1 && DashDelay == 0) {
				Vector2 newVelocity = Player.velocity;

				switch (DashDir) {
					// Only apply the dash velocity if our current speed in the wanted direction is less than DashVelocity
					case DashUp when Player.velocity.Y > -DashVelocity:
					case DashDown when Player.velocity.Y < DashVelocity: {
							// Y-velocity is set here
							// If the direction requested was DashUp, then we adjust the velocity to make the dash appear "faster" due to gravity being immediately in effect
							// This adjustment is roughly 1.3x the intended dash velocity
							float dashDirection = DashDir == DashDown ? 1 : -1.3f;
							newVelocity.Y = dashDirection * DashVelocity;
							break;
						}
					case DashLeft when Player.velocity.X > -DashVelocity:
					case DashRight when Player.velocity.X < DashVelocity: {
							// X-velocity is set here
							float dashDirection = DashDir == DashRight ? 1 : -1;
							newVelocity.X = dashDirection * DashVelocity;
							break;
						}
					default:
						return; // not moving fast enough, so don't start our dash
				}
				// start our dash
				if (!Player.wet){
					DashDelay = 60 * 3;
				}
				else{
					DashDelay = DashCooldown;
				}
				DashTimer = DashDuration;
				Player.velocity = newVelocity;
				// Here you'd be able to set an effect that happens when the dash first activates
				// Some examples include:  the larger smoke effect from the Master Ninja Gear and Tabi
			}

			if (DashDelay > 0)
				DashDelay--;
			
			if (DashTimer > 0) { // dash is active
				Rectangle tridentHitbox = new Rectangle(
            (int)(Player.position.X + Player.width / 2 - 10), // Adjust the values as needed
            (int)(Player.position.Y + Player.height / 2 - 10),
            20,
            20
        );

        for (int i = 0; i < Main.npc.Length; i++)
        {
            NPC npc = Main.npc[i];
            if (npc.active && !npc.dontTakeDamage && npc.life > 0)
            {
                Rectangle npcHitbox = new Rectangle(
                    (int)npc.position.X,
                    (int)npc.position.Y,
                    npc.width,
                    npc.height
                );

                if (tridentHitbox.Intersects(npcHitbox))
                {
                   NPC.HitInfo info1 = new NPC.HitInfo();
				   info1.Damage = 36;
                    npc.StrikeNPC(info1, true, false);
                }
            }
        }
				
				Player.eocDash = DashTimer;
				Player.armorEffectDrawShadowEOCShield = true;

				// count down frames remaining
				DashTimer--;

				if (DashTimer == 0)
                    {
                        featherfallTimer = 30; // 2 seconds
                    }
			}

			if (featherfallTimer > 0 && Player.wet)
                {
					Player.velocity =  new Vector2(Player.velocity.X, 0f);
                    featherfallTimer--;
                }
		}

		private bool CanUseDash() {
			return TridentEquipped
				&& Player.dashType == DashID.None // player doesn't have Tabi or EoCShield equipped (give priority to those dashes)
				&& !Player.setSolar // player isn't wearing solar armor
				&& !Player.mount.Active; // player isn't mounted, since dashes on a mount look weird
		}
	}
}
    }
