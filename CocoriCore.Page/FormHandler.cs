using System.Threading.Tasks;

namespace CocoriCore
{
    public class FormHandler<TCommand, T> : MessageHandler<Form<TCommand, T>, T>
        where TCommand : IMessage, new()
    {
        private readonly IExecuteHandler executeHandler;
        private readonly IPageMapper mapper;

        public FormHandler(IExecuteHandler executeHandler, IPageMapper mapper)
        {
            this.executeHandler = executeHandler;
            this.mapper = mapper;
        }


        public override async Task<T> ExecuteAsync(Form<TCommand, T> message)
        {
            var response = await executeHandler.ExecuteAsync(message.Command);
            return mapper.Map<T>(message.Command, response);
        }
    }

}