using Grpc.Net.Client;
using CheckService;

namespace Beerest.gRPC
{

    public class CheckServiceClient
    {
        private readonly CheckService.CheckService.CheckServiceClient _client;

        public CheckServiceClient(string address)
        {
            var channel = GrpcChannel.ForAddress(address);
            _client = new CheckService.CheckService.CheckServiceClient(channel);
        }

        public async Task<CreateCheckResponse> CreateCheckAsync(CreateCheckRequest request)
        {
            var response = await _client.CreateCheckAsync(request);
            return response;
        }
    }
}
