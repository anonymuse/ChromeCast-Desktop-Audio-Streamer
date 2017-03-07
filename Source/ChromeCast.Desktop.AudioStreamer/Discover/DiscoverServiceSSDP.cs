﻿using System;
using Rssdp;
using ChromeCast.Desktop.AudioStreamer.Discover.Interfaces;

namespace ChromeCast.Desktop.AudioStreamer.Discover
{
    public class DiscoverServiceSSDP : IDiscoverServiceSSDP
    {
        private const string ChromeCastUpnpDeviceType = "urn:dial-multiscreen-org:device:dial:1";
        private Action<DiscoveredSsdpDevice, SsdpDevice> onDiscovered;

        public void Discover(Action<DiscoveredSsdpDevice, SsdpDevice> onDiscoveredIn)
        {
            onDiscovered = onDiscoveredIn;

            using (var deviceLocator = new SsdpDeviceLocator())
            {
                deviceLocator.NotificationFilter = ChromeCastUpnpDeviceType;
                deviceLocator.DeviceAvailable += OnDeviceAvailable;
                deviceLocator.SearchAsync();
            }
        }

        private async void OnDeviceAvailable(object sender, DeviceAvailableEventArgs e)
        {
            var fullDevice = await e.DiscoveredDevice.GetDeviceInfo();
            onDiscovered?.Invoke(e.DiscoveredDevice, fullDevice);
        }
    }
}
