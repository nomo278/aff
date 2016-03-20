using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace Maps
{
    public class MapMarkerPlacer : MonoBehaviour
    {
        public Texture2D mapIcon;

        OnlineMapsTextureControl control;

        public static Dictionary<string, string> venueAddresses = new Dictionary<string, string>
        {
            { "Sweet Auburn BBQ", "656 North Highland Avenue Northeast, Atlanta, GA 30306" },
            { "Joystick", "427 Edgewood Ave SE, Atlanta, GA 30312"},
            { "Sister Louisa's Church of the Living Room & Ping Pong Emporium", "466 Edgewood Ave SE, Atlanta, GA 30312" },
            { "Clairmont Lounge", "789 Ponce De Leon Ave NE, Atlanta, GA 30306" },
            { "Hand in Hand Pub", "752 North Highland Avenue Northeast, Atlanta, GA 30306" },
            { "Gallery L1", "828 Ralph McGill Blvd NE, Atlanta, GA 30306"},
            { "Callanwolde Fine Arts Center", "980 Briarcliff Rd NE, Atlanta, GA 30306" },
            { "Highland Ballroom", "644 North Highland Avenue Northeast, Atlanta, GA 30306" },
            { "Music Room", "327 Edgewood Ave SE Atlanta, Georgia" },
            { "Paris On Ponce", "716 Ponce De Leon Pl NE, Atlanta, GA 30306" },
            { "Ponce City Market", "Atlanta, GA" },
            { "Rialto Center for the Arts", "80 Forsyth St NW, Atlanta, GA 30303" },
            { "Center for Puppetry Arts", "1404 Spring St NW, Atlanta, GA 30309" },
            { "7 Stages", "1105 Euclid Ave NE, Atlanta, GA 30307"},
            { "Plaza Theatre", "1049 Ponce de Leon Ave NE, Atlanta, GA 30306" },
            { "Hill Theatre", "1943 Pleasant Hill Rd, Duluth, GA 30096"},
            { "Buckhead Theatre", "3110 Roswell Rd NE, Atlanta, GA 30305" },
            { "Serenbe", "10950 Hutceson Ferry Rd, Chattachoochee Hills, GA 30268"}
        };

        public static Dictionary<string, string> inputVenueToVenue = new Dictionary<string, string>
        {
            { "Plaza (Main)", "Plaza Theatre" },
            { "Plaza (Up)", "Plaza Theatre" },
            { "7 Stages", "7 Stages" },
            { "Center Puppetry", "Center for Puppetry Arts" },
            { "Hill Theatre", "Hill Theatre"},
            { "Buckhead Theater", "Buckhead Theatre" },
            { "Serenbe", "Serenbe" }
        };

        Dictionary<string, OnlineMapsMarker> venueMarkers = new Dictionary<string, OnlineMapsMarker>();

        bool locationSet = false;

        // Events
        public delegate void VenueClick(OnlineMapsMarker marker);
        public static event VenueClick OnVenueClick;

        void OnEnable()
        {
            control = GetComponent<OnlineMapsTextureControl>();
            control.allowDefaultMarkerEvents = true;
            control.allowAddMarker3DByN = false;
            control.allowAddMarkerByM = false;

            StartCoroutine(VenueLocationsQuery());
        }

        float apiDelay = 0.25f;
        IEnumerator VenueLocationsQuery()
        {
            yield return new WaitForEndOfFrame();
            control.ClearMarkers();

            foreach (KeyValuePair<string,string> kvp in venueAddresses)
            {
                OnlineMapsGoogleAPIQuery query = OnlineMapsFindLocation.Find(kvp.Value);
                query.venue = kvp.Key;
                query.OnCompleteVenue -= GetLocationVenue;
                query.OnCompleteVenue += GetLocationVenue;
                yield return new WaitForSeconds(apiDelay);
            }
        }

        void GetLocationVenue(string result, string venue)
        {
            Vector2 position = OnlineMapsFindLocation.GetCoordinatesFromResult(result);
            OnlineMapsMarker marker = control.CreateMarker(position, mapIcon);
            marker.venue = venue;
            venueMarkers.Add(venue, marker);
            marker.OnClick -= InitialVenueClick;
            marker.OnClick += InitialVenueClick;
        }

        // How to set map position
        // OnlineMaps.instance.position = position;

        void InitialVenueClick(OnlineMapsMarkerBase marker)
        {
            if (OnVenueClick != null)
                OnVenueClick(((OnlineMapsMarker)marker));
        }
    }
}

