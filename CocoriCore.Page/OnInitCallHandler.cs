using System.Threading.Tasks;

namespace CocoriCore
{
    public class OnInitCallHandler<TQuery, TModel> : MessageHandler<OnInitCall<TQuery, TModel>, TModel>
        where TQuery : IMessage
    {
        private readonly IExecuteHandler executeHandler;
        private readonly IPageMapper mapper;

        public OnInitCallHandler(IExecuteHandler executeHandler, IPageMapper mapper)
        {
            this.executeHandler = executeHandler;
            this.mapper = mapper;
        }

        public override async Task<TModel> ExecuteAsync(OnInitCall<TQuery, TModel> OnInitCallMessage)
        {
            var response = await executeHandler.ExecuteAsync(OnInitCallMessage.Message);
            return mapper.Map<TModel>(OnInitCallMessage.Message, response);
        }
    }
}