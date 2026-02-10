using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace FaultCombat.Content.Items;

public class ArmorPatch : ModSystem
{
    public override void Load()
    {
        On_Player.UpdateArmorSets += ArmorSetsPatch;
    }

    private void ArmorSetsPatch(On_Player.orig_UpdateArmorSets orig, Player self, int i)
    {
        orig(self,i);

        if (self.TryGetModPlayer<FaultPlayer>(out FaultPlayer faultPlayer))
        {
            string set = faultPlayer.UpdateSet(self.head,self.body,self.legs);
            if (set != "")
            {
                self.setBonus += "\n"+set;
            }
        }

    }



    //     public void UpdateArmorSets(int i)
    // 	{
    // 		setBonus = "";
    // 		if (body == 67 && legs == 56 && head >= 103 && head <= 105) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.Shroomite");
    // 			shroomiteStealth = true;
    // 		}

    // 		if ((head == 52 && body == 32 && legs == 31) || (head == 53 && body == 33 && legs == 32) || (head == 54 && body == 34 && legs == 33) || (head == 55 && body == 35 && legs == 34) || (head == 71 && body == 47 && legs == 43) || (head == 166 && body == 173 && legs == 108) || (head == 167 && body == 174 && legs == 109)) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.Wood");
    // 			statDefense++;
    // 		}

    // 		if (head == 278 && body == 246 && legs == 234) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.AshWood");
    // 			ashWoodBonus = true;
    // 		}

    // 		if ((head == 1 && body == 1 && legs == 1) || ((head == 72 || head == 2) && body == 2 && legs == 2) || (head == 47 && body == 28 && legs == 27)) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.MetalTier1");
    // 			statDefense += 2;
    // 		}

    // 		if ((head == 3 && body == 3 && legs == 3) || ((head == 73 || head == 4) && body == 4 && legs == 4) || (head == 48 && body == 29 && legs == 28) || (head == 49 && body == 30 && legs == 29)) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.MetalTier2");
    // 			statDefense += 3;
    // 		}

    // 		if (head == 50 && body == 31 && legs == 30) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.Platinum");
    // 			statDefense += 4;
    // 		}

    // 		if (head == 112 && body == 75 && legs == 64) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.Pumpkin");
    // 			allDamage += 0.1f;
    // 			/*
    // 			meleeDamage += 0.1f;
    // 			magicDamage += 0.1f;
    // 			rangedDamage += 0.1f;
    // 			minionDamage += 0.1f;
    // 			*/
    // 		}

    // 		if (head == 180 && body == 182 && legs == 122) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.Gladiator");
    // 			noKnockback = true;
    // 		}

    // 		if (head == 22 && body == 14 && legs == 14) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.Ninja");
    // 			moveSpeed += 0.2f;
    // 		}

    // 		if (head == 188 && body == 189 && legs == 129) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.Fossil");
    // 			ammoCost80 = true;
    // 		}

    // 		if ((head == 75 || head == 7) && body == 7 && legs == 7) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.Bone");
    // 			rangedCrit += 10;
    // 		}

    // 		if (head == 157 && body == 105 && legs == 98) {
    // 			int num = 0;
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.BeetleDamage");
    // 			beetleOffense = true;
    // 			beetleCounter -= 3f;
    // 			beetleCounter -= beetleCountdown / 20;
    // 			beetleCountdown++;
    // 			if (beetleCounter < 0f)
    // 				beetleCounter = 0f;

    // 			int num2 = 400;
    // 			int num3 = 1200;
    // 			int num4 = 3600;
    // 			if (beetleCounter > (float)(num2 + num3 + num4 + num3))
    // 				beetleCounter = num2 + num3 + num4 + num3;

    // 			if (beetleCounter > (float)(num2 + num3 + num4)) {
    // 				AddBuff(100, 5, quiet: false);
    // 				num = 3;
    // 			}
    // 			else if (beetleCounter > (float)(num2 + num3)) {
    // 				AddBuff(99, 5, quiet: false);
    // 				num = 2;
    // 			}
    // 			else if (beetleCounter > (float)num2) {
    // 				AddBuff(98, 5, quiet: false);
    // 				num = 1;
    // 			}

    // 			if (num < beetleOrbs)
    // 				beetleCountdown = 0;
    // 			else if (num > beetleOrbs)
    // 				beetleCounter += 200f;

    // 			if (num != beetleOrbs && beetleOrbs > 0) {
    // 				for (int j = 0; j < maxBuffs; j++) {
    // 					if (buffType[j] >= 98 && buffType[j] <= 100 && buffType[j] != 97 + num)
    // 						DelBuff(j);
    // 				}
    // 			}
    // 		}
    // 		else if (head == 157 && body == 106 && legs == 98) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.BeetleDefense");
    // 			beetleDefense = true;
    // 			beetleCounter += 1f;
    // 			int num5 = 180;
    // 			if (beetleCounter >= (float)num5) {
    // 				if (beetleOrbs > 0 && beetleOrbs < 3) {
    // 					for (int k = 0; k < maxBuffs; k++) {
    // 						if (buffType[k] >= 95 && buffType[k] <= 96)
    // 							DelBuff(k);
    // 					}
    // 				}

    // 				if (beetleOrbs < 3) {
    // 					AddBuff(95 + beetleOrbs, 5, quiet: false);
    // 					beetleCounter = 0f;
    // 				}
    // 				else {
    // 					beetleCounter = num5;
    // 				}
    // 			}
    // 		}

    // 		if (!beetleDefense && !beetleOffense) {
    // 			beetleCounter = 0f;
    // 		}
    // 		else {
    // 			beetleFrameCounter++;
    // 			if (beetleFrameCounter >= 1) {
    // 				beetleFrameCounter = 0;
    // 				beetleFrame++;
    // 				if (beetleFrame > 2)
    // 					beetleFrame = 0;
    // 			}

    // 			for (int l = beetleOrbs; l < 3; l++) {
    // 				beetlePos[l].X = 0f;
    // 				beetlePos[l].Y = 0f;
    // 			}

    // 			for (int m = 0; m < beetleOrbs; m++) {
    // 				beetlePos[m] += beetleVel[m];
    // 				beetleVel[m].X += (float)Main.rand.Next(-100, 101) * 0.005f;
    // 				beetleVel[m].Y += (float)Main.rand.Next(-100, 101) * 0.005f;
    // 				float x = beetlePos[m].X;
    // 				float y = beetlePos[m].Y;
    // 				float num6 = (float)Math.Sqrt(x * x + y * y);
    // 				if (num6 > 100f) {
    // 					num6 = 20f / num6;
    // 					x *= 0f - num6;
    // 					y *= 0f - num6;
    // 					int num7 = 10;
    // 					beetleVel[m].X = (beetleVel[m].X * (float)(num7 - 1) + x) / (float)num7;
    // 					beetleVel[m].Y = (beetleVel[m].Y * (float)(num7 - 1) + y) / (float)num7;
    // 				}
    // 				else if (num6 > 30f) {
    // 					num6 = 10f / num6;
    // 					x *= 0f - num6;
    // 					y *= 0f - num6;
    // 					int num8 = 20;
    // 					beetleVel[m].X = (beetleVel[m].X * (float)(num8 - 1) + x) / (float)num8;
    // 					beetleVel[m].Y = (beetleVel[m].Y * (float)(num8 - 1) + y) / (float)num8;
    // 				}

    // 				x = beetleVel[m].X;
    // 				y = beetleVel[m].Y;
    // 				num6 = (float)Math.Sqrt(x * x + y * y);
    // 				if (num6 > 2f)
    // 					beetleVel[m] *= 0.9f;

    // 				beetlePos[m] -= velocity * 0.25f;
    // 			}
    // 		}

    // 		if (head == 14 && ((body >= 58 && body <= 63) || body == 167 || body == 213)) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.Wizard");
    // 			magicCrit += 10;
    // 		}

    // 		if (head == 159 && ((body >= 58 && body <= 63) || body == 167 || body == 213)) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.MagicHat");
    // 			statManaMax2 += 60;
    // 		}

    // 		if ((head == 5 || head == 74) && (body == 5 || body == 48) && (legs == 5 || legs == 44)) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.ShadowScale");
    // 			shadowArmor = true;
    // 		}

    // 		if (head == 57 && body == 37 && legs == 35) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.Crimson");
    // 			crimsonRegen = true;
    // 		}

    // 		if (head == 101 && body == 66 && legs == 55) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.SpectreHealing");
    // 			ghostHeal = true;
    // 			magicDamage -= 0.4f;
    // 		}

    // 		if (head == 156 && body == 66 && legs == 55) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.SpectreDamage");
    // 			ghostHurt = true;
    // 		}

    // 		if (head == 6 && body == 6 && legs == 6) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.Meteor");
    // 			spaceGun = true;
    // 		}

    // 		if (head == 46 && body == 27 && legs == 26) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.Frost");
    // 			frostBurn = true;
    // 			meleeDamage += 0.1f;
    // 			rangedDamage += 0.1f;
    // 		}

    // 		if ((head == 76 || head == 8) && (body == 49 || body == 8) && (legs == 45 || legs == 8)) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.Jungle");
    // 			manaCost -= 0.16f;
    // 		}

    // 		if (head == 9 && body == 9 && legs == 9) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.Molten");
    // 			meleeDamage += 0.1f;
    // 			buffImmune[24] = true;
    // 		}

    // 		if ((head == 58 || head == 77) && (body == 38 || body == 50) && (legs == 36 || legs == 46)) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.Snow");
    // 			buffImmune[46] = true;
    // 			buffImmune[47] = true;
    // 		}

    // 		if (head == 11 && body == 20 && legs == 19) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.Mining");
    // 			pickSpeed -= 0.1f;
    // 		}

    // 		if (head == 216 && body == 20 && legs == 19) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.Mining");
    // 			pickSpeed -= 0.1f;
    // 		}

    // 		if (head == 78 && body == 51 && legs == 47) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.ChlorophyteMelee");
    // 			AddBuff(60, 18000);
    // 			endurance += 0.05f;
    // 		}
    // 		else if ((head == 80 || head == 79) && body == 51 && legs == 47) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.Chlorophyte");
    // 			AddBuff(60, 18000);
    // 		}
    // 		else if (crystalLeaf) {
    // 			for (int n = 0; n < maxBuffs; n++) {
    // 				if (buffType[n] == 60)
    // 					DelBuff(n);
    // 			}
    // 		}

    // 		if (head == 161 && body == 169 && legs == 104) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.Angler");
    // 			anglerSetSpawnReduction = true;
    // 		}

    // 		if (head == 70 && body == 46 && legs == 42) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.Cactus");
    // 			cactusThorns = true;
    // 		}

    // 		if (head == 99 && body == 65 && legs == 54) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.Turtle");
    // 			endurance += 0.15f;
    // 			thorns = 1f;
    // 			turtleThorns = true;
    // 		}

    // 		if (body == 17 && legs == 16) {
    // 			if (head == 29) {
    // 				setBonus = Language.GetTextValue("ArmorSetBonus.CobaltCaster");
    // 				manaCost -= 0.14f;
    // 			}
    // 			else if (head == 30) {
    // 				setBonus = Language.GetTextValue("ArmorSetBonus.CobaltMelee");
    // 				meleeSpeed += 0.15f;
    // 			}
    // 			else if (head == 31) {
    // 				setBonus = Language.GetTextValue("ArmorSetBonus.CobaltRanged");
    // 				ammoCost80 = true;
    // 			}
    // 		}

    // 		if (body == 18 && legs == 17) {
    // 			if (head == 32) {
    // 				setBonus = Language.GetTextValue("ArmorSetBonus.MythrilCaster");
    // 				manaCost -= 0.17f;
    // 			}
    // 			else if (head == 33) {
    // 				setBonus = Language.GetTextValue("ArmorSetBonus.MythrilMelee");
    // 				meleeCrit += 10;
    // 			}
    // 			else if (head == 34) {
    // 				setBonus = Language.GetTextValue("ArmorSetBonus.MythrilRanged");
    // 				ammoCost80 = true;
    // 			}
    // 		}

    // 		if (body == 19 && legs == 18) {
    // 			if (head == 35) {
    // 				setBonus = Language.GetTextValue("ArmorSetBonus.AdamantiteCaster");
    // 				manaCost -= 0.19f;
    // 			}
    // 			else if (head == 36) {
    // 				setBonus = Language.GetTextValue("ArmorSetBonus.AdamantiteMelee");
    // 				meleeSpeed += 0.2f;
    // 				moveSpeed += 0.2f;
    // 			}
    // 			else if (head == 37) {
    // 				setBonus = Language.GetTextValue("ArmorSetBonus.AdamantiteRanged");
    // 				ammoCost75 = true;
    // 			}
    // 		}

    // 		if (body == 54 && legs == 49 && (head == 83 || head == 84 || head == 85)) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.Palladium");
    // 			onHitRegen = true;
    // 		}

    // 		if (body == 55 && legs == 50 && (head == 86 || head == 87 || head == 88)) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.Orichalcum");
    // 			onHitPetal = true;
    // 		}

    // 		if (body == 56 && legs == 51) {
    // 			bool flag = false;
    // 			if (head == 91) {
    // 				setBonus = Language.GetTextValue("ArmorSetBonus.Titanium");
    // 				flag = true;
    // 			}
    // 			else if (head == 89) {
    // 				setBonus = Language.GetTextValue("ArmorSetBonus.Titanium");
    // 				flag = true;
    // 			}
    // 			else if (head == 90) {
    // 				setBonus = Language.GetTextValue("ArmorSetBonus.Titanium");
    // 				flag = true;
    // 			}

    // 			if (flag)
    // 				onHitTitaniumStorm = true;
    // 		}

    // 		if ((body == 24 || body == 229) && (legs == 23 || legs == 212) && (head == 42 || head == 41 || head == 43 || head == 254 || head == 257 || head == 256 || head == 255 || head == 258)) {
    // 			if (head == 254 || head == 258) {
    // 				setBonus = Language.GetTextValue("ArmorSetBonus.HallowedSummoner");
    // 				maxMinions += 2;
    // 			}
    // 			else {
    // 				setBonus = Language.GetTextValue("ArmorSetBonus.Hallowed");
    // 			}

    // 			onHitDodge = true;
    // 		}

    // 		if (head == 261 && body == 230 && legs == 213) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.CrystalNinja");
    // 			allDamage += 0.1f;
    // 			allCrit += 10;
    // 			/*
    // 			rangedDamage += 0.1f;
    // 			meleeDamage += 0.1f;
    // 			magicDamage += 0.1f;
    // 			minionDamage += 0.1f;
    // 			rangedCrit += 10;
    // 			meleeCrit += 10;
    // 			magicCrit += 10;
    // 			*/
    // 			dashType = 5;
    // 		}

    // 		if (head == 82 && body == 53 && legs == 48) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.Tiki");
    // 			maxMinions++;
    // 			whipRangeMultiplier += 0.2f;
    // 		}

    // 		if (head == 134 && body == 95 && legs == 79) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.Spooky");
    // 			minionDamage += 0.25f;
    // 		}

    // 		if (head == 160 && body == 168 && legs == 103) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.Bee");
    // 			minionDamage += 0.1f;
    // 			if (itemAnimation > 0 && inventory[selectedItem].type == 1121)
    // 				AchievementsHelper.HandleSpecialEvent(this, 3);
    // 		}

    // 		if (head == 162 && body == 170 && legs == 105) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.Spider");
    // 			minionDamage += 0.12f;
    // 		}

    // 		if (head == 171 && body == 177 && legs == 112) {
    // 			endurance += 0.12f;
    // 			setSolar = true;
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.Solar");
    // 			solarCounter++;
    // 			int num9 = 180;
    // 			if (solarCounter >= num9) {
    // 				if (solarShields > 0 && solarShields < 3) {
    // 					for (int num10 = 0; num10 < maxBuffs; num10++) {
    // 						if (buffType[num10] >= 170 && buffType[num10] <= 171)
    // 							DelBuff(num10);
    // 					}
    // 				}

    // 				if (solarShields < 3) {
    // 					AddBuff(170 + solarShields, 5, quiet: false);
    // 					for (int num11 = 0; num11 < 16; num11++) {
    // 						Dust obj = Main.dust[Dust.NewDust(position, width, height, 6, 0f, 0f, 100)];
    // 						obj.noGravity = true;
    // 						obj.scale = 1.7f;
    // 						obj.fadeIn = 0.5f;
    // 						obj.velocity *= 5f;
    // 						obj.shader = GameShaders.Armor.GetSecondaryShader(ArmorSetDye(), this);
    // 					}

    // 					solarCounter = 0;
    // 				}
    // 				else {
    // 					solarCounter = num9;
    // 				}
    // 			}

    // 			for (int num12 = solarShields; num12 < 3; num12++) {
    // 				solarShieldPos[num12] = Vector2.Zero;
    // 			}

    // 			for (int num13 = 0; num13 < solarShields; num13++) {
    // 				solarShieldPos[num13] += solarShieldVel[num13];
    // 				Vector2 vector = ((float)miscCounter / 100f * ((float)Math.PI * 2f) + (float)num13 * ((float)Math.PI * 2f / (float)solarShields)).ToRotationVector2() * 6f;
    // 				vector.X = direction * 20;
    // 				if (mount.Active && mount.Type == 52)
    // 					vector.X = direction * 50;

    // 				solarShieldVel[num13] = (vector - solarShieldPos[num13]) * 0.2f;
    // 			}

    // 			if (dashDelay >= 0) {
    // 				solarDashing = false;
    // 				solarDashConsumedFlare = false;
    // 			}

    // 			bool flag2 = solarDashing && dashDelay < 0;
    // 			if (solarShields > 0 || flag2)
    // 				dashType = 3;
    // 		}
    // 		else {
    // 			solarCounter = 0;
    // 		}

    // 		if (head == 169 && body == 175 && legs == 110) {
    // 			setVortex = true;
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.Vortex", Language.GetTextValue(Main.ReversedUpDownArmorSetBonuses ? "Key.UP" : "Key.DOWN"));
    // 		}
    // 		else {
    // 			vortexStealthActive = false;
    // 		}

    // 		if (head == 170 && body == 176 && legs == 111) {
    // 			if (nebulaCD > 0)
    // 				nebulaCD--;

    // 			setNebula = true;
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.Nebula");
    // 		}

    // 		if (head == 189 && body == 190 && legs == 130) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.Stardust", Language.GetTextValue(Main.ReversedUpDownArmorSetBonuses ? "Key.UP" : "Key.DOWN"));
    // 			setStardust = true;
    // 			if (whoAmI == Main.myPlayer) {
    // 				if (FindBuffIndex(187) == -1)
    // 					AddBuff(187, 3600);

    // 				if (ownedProjectileCounts[623] < 1) {
    // 					int num14 = 10;
    // 					int num15 = 30;
    // 					int num16 = Projectile.NewProjectile(GetProjectileSource_SetBonus(7), base.Center.X, base.Center.Y, 0f, -1f, 623, num15, num14, Main.myPlayer);
    // 					Main.projectile[num16].originalDamage = num15;
    // 				}
    // 			}
    // 		}
    // 		else if (FindBuffIndex(187) != -1) {
    // 			DelBuff(FindBuffIndex(187));
    // 		}

    // 		if (head == 200 && body == 198 && legs == 142) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.Forbidden", Language.GetTextValue(Main.ReversedUpDownArmorSetBonuses ? "Key.UP" : "Key.DOWN"));
    // 			setForbidden = true;
    // 			UpdateForbiddenSetLock();
    // 			Lighting.AddLight(base.Center, 0.8f, 0.7f, 0.2f);
    // 		}

    // 		if (head == 204 && body == 201 && legs == 145) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.SquireTier2");
    // 			setSquireT2 = true;
    // 			maxTurrets++;
    // 		}

    // 		if (head == 203 && body == 200 && legs == 144) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.ApprenticeTier2");
    // 			setApprenticeT2 = true;
    // 			maxTurrets++;
    // 		}

    // 		if (head == 205 && body == 202 && (legs == 147 || legs == 146)) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.HuntressTier2");
    // 			setHuntressT2 = true;
    // 			maxTurrets++;
    // 		}

    // 		if (head == 206 && body == 203 && legs == 148) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.MonkTier2");
    // 			setMonkT2 = true;
    // 			maxTurrets++;
    // 		}

    // 		if (head == 210 && body == 204 && legs == 152) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.SquireTier3");
    // 			setSquireT3 = true;
    // 			setSquireT2 = true;
    // 			maxTurrets++;
    // 		}

    // 		if (head == 211 && body == 205 && legs == 153) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.ApprenticeTier3");
    // 			setApprenticeT3 = true;
    // 			setApprenticeT2 = true;
    // 			maxTurrets++;
    // 		}

    // 		if (head == 212 && body == 206 && (legs == 154 || legs == 155)) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.HuntressTier3");
    // 			setHuntressT3 = true;
    // 			setHuntressT2 = true;
    // 			maxTurrets++;
    // 		}

    // 		if (head == 213 && body == 207 && legs == 156) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.MonkTier3");
    // 			setMonkT3 = true;
    // 			setMonkT2 = true;
    // 			maxTurrets++;
    // 		}

    // 		if (head == 185 && body == 187 && legs == 127) {
    // 			setBonus = Language.GetTextValue("ArmorSetBonus.ObsidianOutlaw");
    // 			minionDamage += 0.15f;
    // 			whipRangeMultiplier += 0.3f;
    // 			float num17 = 1.15f;
    // 			/*
    // 			float num18 = 1f / num17;
    // 			whipUseTimeMultiplier *= num18;
    // 			*/
    // 			summonerWeaponSpeedBonus += num17 - 1; //TML: Obsidian armor changed to additive.
    // 		}

    // 		ApplyArmorSoundAndDustChanges();

    // 		ItemLoader.UpdateArmorSet(this, armor[0], armor[1], armor[2]);
    // 	}
}