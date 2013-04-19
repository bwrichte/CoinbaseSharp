CoinbaseSharp
=============

CoinbaseSharp is a Coinbase Client Library for the .NET framework.

This API is designed currently to only use the API_KEY authentication mechanism,
and is thus good for managing single or small sets of accounts for which the
API_KEY is known.  It will eventually be expanded to use OAuth2.

In order to use this API you must know your API_KEY: 
https://coinbase.com/docs/api/overview#api_key

To use this client library, simply add
CoinbaseSharp/bin/Release/CoinbaseSharp.dll
as a reference to your project.

Then, add the following using statements to your code:

using CoinbaseSharp.API;
using CoinbaseSharp.Authentication;
using CoinbaseSharp.DataTypes;
using CoinbaseSharp.DataTypes.Responses;
using CoinbaseSharp.Resources;

Then you can access CoinbaseSharp resources, buy/sell/transfer Bitcoins, sign
up new users, get currency information, create payment buttons, etc. by using
the API.* methods which abstract away the HTTP/JSON Coinbase protcol.

All APIs listed for Version 1 (https://coinbase.com/api/doc) are implemented.

Moreover, until additional documentation is added, these docs can serve as
a standin for how the CoinbaseResource objects are structured.
