﻿using Cysharp.Threading.Tasks;
using SDG.Unturned;
using System;
using UnityEngine;

namespace Alpalis
{
    public static class PlayerExtensions
    {
        private static readonly byte[] PlaceholderArray = Array.Empty<byte>();

        public static async UniTask<bool> TeleportToLocationAsync(this Player player, Vector3 position) =>
            await TeleportToLocationAsync(player, position, player.transform.eulerAngles.y);

        public static async UniTask<bool> TeleportToLocationAsync(this Player player, Vector3 position, float yaw)
        {
            await UniTask.SwitchToMainThread();
            return player.teleportToLocation(position, yaw);
        }

        public static async UniTask TeleportToLocationUnsafeAsync(this Player player, Vector3 position)
        {
            await UniTask.SwitchToMainThread();
            player.teleportToLocationUnsafe(position, player.transform.eulerAngles.y);
        }
        public static void ClearInventory(this Player player)
        {
            Items[] items = player.inventory.items;
            for (byte b = 0; b < PlayerInventory.PAGES - 2; b++)
            {
                if (items[b]?.items?.Count == 0)
                {
                    continue;
                }

                items[b].ReverseClear();

                if (b < PlayerInventory.SLOTS)
                {
                    player.equipment.sendSlot(b);
                }
            }

            PlayerClothing clothing = player.clothing;
            HumanClothes clothes = clothing.thirdClothes;
            if (clothing.backpack != 0)
            {
                clothes.backpack = 0;
                clothing.askWearBackpack(0, 0, PlaceholderArray, true);
            }

            if (clothing.glasses != 0)
            {
                clothes.glasses = 0;
                clothing.askWearGlasses(0, 0, PlaceholderArray, true);
            }

            if (clothing.hat != 0)
            {
                clothes.hat = 0;
                clothing.askWearHat(0, 0, PlaceholderArray, true);
            }

            if (clothing.mask != 0)
            {
                clothes.mask = 0;
                clothing.askWearMask(0, 0, PlaceholderArray, true);
            }

            if (clothing.pants != 0)
            {
                clothes.pants = 0;
                clothing.askWearPants(0, 0, PlaceholderArray, true);
            }

            if (clothing.shirt != 0)
            {
                clothes.shirt = 0;
                clothing.askWearShirt(0, 0, PlaceholderArray, true);
            }

            if (clothing.vest != 0)
            {
                clothes.vest = 0;
                clothing.askWearVest(0, 0, PlaceholderArray, true);
            }
        }
    }
}
