using Confluent.Kafka;

namespace EmployeeWebApi.kafka
{
    public class ConsumerService:BackgroundService
    {
        private readonly IConsumer<Null, string> _consumer;

        private readonly ILogger<ConsumerService> _logger;

        private const string DepartmentTopic = "Department";

        public ConsumerService(IConfiguration configuration, ILogger<ConsumerService> logger)
        {
            _logger = logger;

            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                GroupId = "department-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Null, string>(consumerConfig).Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe(DepartmentTopic);
            while (true)
            {
                var response = _consumer.Consume(stoppingToken);

                if (!string.IsNullOrEmpty(response.Message.Value))
                {
                    Console.Write($"{response.Message.Value}");
                }

            }
        }
        public override void Dispose()
        {
            _consumer.Close();
            _consumer.Dispose();

            base.Dispose();
        }

        public void ProcessKafkaMessage(CancellationToken stoppingToken)
        {
            try
            {
                var consumeResult = _consumer.Consume(stoppingToken);

                var message = consumeResult.Message.Value;

                _logger.LogInformation($"{message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing Kafka message: {ex.Message}");
            }
        }

    }
}
