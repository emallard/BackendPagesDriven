using System.Threading.Tasks;

namespace CocoriCore
{
    public class ActionCallHandler<TQuery, TModel> : MessageHandler<ActionCall<TQuery, TModel>, TModel>
        where TQuery : IMessage
    {
        private readonly IExecuteHandler executeHandler;
        private readonly IPageMapper mapper;

        public ActionCallHandler(IExecuteHandler executeHandler, IPageMapper mapper)
        {
            this.executeHandler = executeHandler;
            this.mapper = mapper;
        }

        public override async Task<TModel> ExecuteAsync(ActionCall<TQuery, TModel> ActionCallMessage)
        {
            var response = await executeHandler.ExecuteAsync(ActionCallMessage.Message);
            return mapper.Map<TModel>(ActionCallMessage.Message, response);
        }
    }
}