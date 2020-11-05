// Copyright © 2020 Sovren Group, Inc. All rights reserved.
// This file is provided for use by, or on behalf of, Sovren licensees
// within the terms of their license of Sovren products or Sovren customers
// within the Terms of Service pertaining to the Sovren SaaS products.

namespace Sovren.Models.API.Matching.UI.Hooks
{
    /// <summary>
    /// A base class for both kinds of hooks
    /// </summary>
    public class UserActionHook
    {
        /// <summary>
        /// Text to display on the button for the user action.
        /// </summary>
        public string LinkText { get; set; }

        /// <summary>
        /// Set to <see langword="true"/> to allow users to select multiple documents and perform this action on all of them at once. 
        /// <br/>NOTE: this can only be set to <see langword="true"/> when you use a <see cref="JsAction"/>. <see cref="UrlAction"/>s are not supported. 
        /// <br/>See <see href="https://docs.sovren.com/Documentation/AIMatching#ui-match-hooks">here</see> for more info.
        /// </summary>
        public bool IsBulk { get; set; }
    }

    /// <summary>
    /// A hook that does some client-side action (opening a tab/window, running some javascript)
    /// </summary>
    public class ClientSideHook : UserActionHook
    {
        /// <summary>
        /// A Javascript action to perform when the user clicks the button. This will post a Javascript
        /// message back to the parent/opener window so that the integrator can run some Javascript.
        /// <br/>This uses <code>window.postMessage()</code>
        /// <br/>NOTE: you can use this or <see cref="UrlAction"/> but not both
        /// </summary>
        public JsAction JsAction { get; set; }

        /// <summary>
        /// A URL action to perform when the user clicks the button. This can open a new window or redirect an existing window to a URL.
        /// <br/>NOTE: you can use this or <see cref="JsAction"/> but not both
        /// </summary>
        public UrlAction UrlAction { get; set; }
    }

    /// <summary>
    /// A hook that does some server-side action (performs an HTTP POST to your server)
    /// </summary>
    public class ServerSideHook : UserActionHook
    {
        /// <summary>
        /// The URL for an HTTP POST call to perform some action in your system.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Any data from your system that you need to associate with this session/action. 
        /// This is sent (in addition to document information) in the POST body. 
        /// <br/>For more information see <see href="https://docs.sovren.com/Documentation/AIMatching#ui-match-hooks">here</see>.
        /// </summary>
        public object CustomInfo { get; set; }
    }
}