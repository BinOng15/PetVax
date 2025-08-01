﻿using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Services.Service
{
    public class MapBoxService
    {
        private readonly string _accessToken;

        public MapBoxService(IConfiguration configuration)
        {
            _accessToken = configuration["MapBox:AccessToken"];
        }

        public async Task<(double lat, double lng)?> GetCoordinatesAsync(string address)
        {
            using var client = new HttpClient();
            var url = $"https://api.mapbox.com/geocoding/v5/mapbox.places/{Uri.EscapeDataString(address)}.json?access_token={_accessToken}";
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            var obj = JObject.Parse(json);
            var features = obj["features"];
            if (features == null || !features.Any()) return null;

            var coordinates = features[0]["geometry"]?["coordinates"];
            if (coordinates == null) return null;

            double lng = (double)coordinates[0];
            double lat = (double)coordinates[1];
            return (lat, lng);
        }

        public async Task<string?> GetAddressAsync(double lat, double lng)
        {
            using var client = new HttpClient();
            var url = $"https://api.mapbox.com/geocoding/v5/mapbox.places/{lng},{lat}.json?access_token={_accessToken}";
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            var obj = JObject.Parse(json);
            var features = obj["features"];
            if (features == null || !features.Any()) return null;

            // Get the most relevant address (first feature)
            var placeName = features[0]?["place_name"]?.ToString();
            return placeName;
        }

        public async Task<(double lat, double lng)?> GetFptHcmCoordinatesAsync()
        {
            const string defaultAddress = "Trường Đại Học FPT Thành Phố Hồ Chí Minh";
            return await GetCoordinatesAsync(defaultAddress);
        }
    }
}
