# Net.Torrent
[![Build Status](https://travis-ci.com/l0nley/Net.UdpServer.svg?branch=master)](https://travis-ci.com/l0nley/Net.UdpServer)
Net.UdpServer is simple Udp server, wich allows implement message processing pipeline for custom UDP-based protocol. 
General architecture is similar to asp.net core middlewares.
Features:
  - Simple Pipeline builder, which allows to build any pipeline you want
  - Simple server, based on UdpClient to receive and send messages
  - DI Support
  - Request context factory, to create protocol specific request contexts

### Examples
Create simple echo service
```csharp
var services = new ServiceCollection();
services.AddLogging(); // for example using Microsoft.Extensions.Logging.Console;
services.AddOptions<ServerConfiguration>(config => {
	config.EndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 1337);
});
services.AddDefaultRequestContextFactory();
services.AddSingleton<IServer,Server>();
var provider = services.BuildServiceProvider();
var pipeline = new PipelineBuilder(provider)
	.Use(async (context, next) => {
		context.ResponsePacket = context.RequestPacket;
	})
	.Build();
var server = provider.GetRequiredService<IServer>();
server.Run(pipeline, CancellationToken.None);
```
### Development
Feel free to fork and improve. I will be very pleased by pull requests.
### License
Copyright 2018 Uladzimir Harabtsou

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.