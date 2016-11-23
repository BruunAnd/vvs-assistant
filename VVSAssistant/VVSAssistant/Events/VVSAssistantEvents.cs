﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Models;

namespace VVSAssistant.Events
{
    public static class VVSAssistantEvents
    {
        /* When the "Gem" button in the GenerateOfferDialogView is pressed */
        public delegate void SaveOfferButtonPressed(Offer offer);
        public static event SaveOfferButtonPressed SaveOfferButtonPressedEventHandler;
        public static void OnSaveOfferButtonPressed(Offer offer)
        {
            SaveOfferButtonPressedEventHandler?.Invoke(offer);
        }
    }
}