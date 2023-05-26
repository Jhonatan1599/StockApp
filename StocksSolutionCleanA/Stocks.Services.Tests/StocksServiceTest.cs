using AutoFixture;
//using EntityFrameworkCoreMock;
using FluentAssertions;
//using Microsoft.EntityFrameworkCore;
using Moq;
using ServiceContracts.DTO;
using ServiceContracts.StocksService;
using Services;
using Services.StocksService;
using Stocks.Core.Domain.Entities;
using Stocks.Core.Domain.RepositoryContracts;
using System.Xml.Linq;
namespace Tests
{
    public class StocksServiceTest
    {
        private readonly IStocksSellOrderService _stocksSellOrderService;
        private readonly IStocksBuyOrderService _stocksBuyOrderService;

        private readonly IFixture _fixture;

        //mock the repository
        private readonly IStocksRepository _stocksRepository;
        private readonly Mock<IStocksRepository> _stocksRepositoryMock;

        public StocksServiceTest()
        {
            _fixture = new Fixture();

            //create the mock object
            _stocksRepositoryMock = new Mock<IStocksRepository>();
            //create a false repository object
            _stocksRepository = _stocksRepositoryMock.Object;

            //Data source instead a SQL db
            //var buyOrders = new List<BuyOrder>();
            //var sellOrders = new List<SellOrder>();

            // Create a mock object for the ApplicationDbContext
            //DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);

            // Get the dbContext
            //ApplicationDbContext dbContext = dbContextMock.Object;

            //Mock the dbSet
            //dbContextMock.CreateDbSetMock(temp => temp.BuyOrders, buyOrders);
            //dbContextMock.CreateDbSetMock(temp => temp.SellOrders, sellOrders);

            _stocksSellOrderService = new StocksSellOrderService(_stocksRepository);
            _stocksBuyOrderService = new StocksBuyOrderService(_stocksRepository);


        }



        #region CreateBuyOrder


        [Fact]
        public async Task CreateBuyOrder_NullBuyOrder_ToBeArgumentNullException()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = null;

            //Act
            //await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            //{
            //    await _stocksService.CreateBuyOrder(buyOrderRequest);
            //});
            // Act
            Func<Task> action = async () =>
            {
                await _stocksBuyOrderService.CreateBuyOrder(buyOrderRequest);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>();

        }



        [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
        [InlineData(0)] //passing parameters to the tet method
        public async Task CreateBuyOrder_QuantityIsLessThanMinimum_ToBeArgumentException(uint buyOrderQuantity)
        {
            //Arrange
            //BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = 1, Quantity = buyOrderQuantity };
            //Act
            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    await _stocksService.CreateBuyOrder(buyOrderRequest);
            //});

            // Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>().With(temp => temp.Quantity, buyOrderQuantity).Create();

            // Act
            Func<Task> action = async () =>
            {
                await _stocksBuyOrderService.CreateBuyOrder(buyOrderRequest);
            };

            // Assert

            await action.Should().ThrowAsync<ArgumentException>();

        }


        [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
        [InlineData(100001)] //passing parameters to the tet method
        public async Task CreateBuyOrder_QuantityIsGreaterThanMaximum_ToBeArgumentException(uint buyOrderQuantity)
        {
            //Arrange
            //BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = 1, Quantity = buyOrderQuantity };
            //Act
            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    await _stocksService.CreateBuyOrder(buyOrderRequest);
            //});

            //Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>().With(temp => temp.Quantity, buyOrderQuantity).Create();

            //Act
            Func<Task> action = async () =>
            {
                await _stocksBuyOrderService.CreateBuyOrder(buyOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }


        [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
        [InlineData(0)] //passing parameters to the tet method
        public async Task CreateBuyOrder_PriceIsLessThanMinimum_ToBeArgumentException(uint buyOrderPrice)
        {
            //Arrange
            //BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = buyOrderPrice, Quantity = 1 };
            //Act
            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    await _stocksService.CreateBuyOrder(buyOrderRequest);
            //});
            // Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>().With(temp => temp.Price, buyOrderPrice).Create();

            //Act
            Func<Task> action = async () =>
            {
                await _stocksBuyOrderService.CreateBuyOrder(buyOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }


        [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
        [InlineData(10001)] //passing parameters to the tet method
        public async Task CreateBuyOrder_PriceIsGreaterThanMaximum_ToBeArgumentException(uint buyOrderPrice)
        {
            //Arrange
            //BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = buyOrderPrice, Quantity = 1 };
            //Act
            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    await _stocksService.CreateBuyOrder(buyOrderRequest);
            //});
            // Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>().With(temp => temp.Price, buyOrderPrice).Create();

            //Act
            Func<Task> action = async () =>
            {
                await _stocksBuyOrderService.CreateBuyOrder(buyOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();

        }


        [Fact]
        public async Task CreateBuyOrder_StockSymbolIsNull_ToBeArgumentException()
        {
            //Arrange
            //BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() { StockSymbol = null, StockName = "Microsoft", Price = 1, Quantity = 1 };

            //Act
            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    await _stocksService.CreateBuyOrder(buyOrderRequest);
            //});

            // Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>().With(temp => temp.StockSymbol, null as string).Create();

            //Act
            Func<Task> action = async () =>
            {
                await _stocksBuyOrderService.CreateBuyOrder(buyOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }


        [Fact]
        public async Task CreateBuyOrder_DateOfOrderIsLessThanYear2000_ToBeArgumentException()
        {
            //Arrange
            //BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", DateAndTimeOfOrder = Convert.ToDateTime("1999-12-31"), Price = 1, Quantity = 1 };

            //Act
            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    await _stocksService.CreateBuyOrder(buyOrderRequest);
            //});
            // Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>().With(temp => temp.DateAndTimeOfOrder, Convert.ToDateTime("1999-12-31")).Create();

            //Act
            Func<Task> action = async () =>
            {
                await _stocksBuyOrderService.CreateBuyOrder(buyOrderRequest);
            };
            //BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(buyOrderRequest);


            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }


        [Fact]
        public async Task CreateBuyOrder_ValidData_ToBeSuccessful()
        {
            //Arrange
            //BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", DateAndTimeOfOrder = Convert.ToDateTime("2024-12-31"), Price = 1, Quantity = 1 };
            //Act
            //BuyOrderResponse buyOrderResponseFromCreate = await _stocksService.CreateBuyOrder(buyOrderRequest);
            //Assert
            //Assert.NotEqual(Guid.Empty, buyOrderResponseFromCreate.BuyOrderID);

            // Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>().Create();

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            BuyOrderResponse buyOrderResponseExpected = buyOrder.ToBuyOrderResponse();

            //_stocksRepositoryMock.Setup(temp => temp.AddBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            //Act
            BuyOrderResponse buyOrderResponseFromCreate = await _stocksBuyOrderService.CreateBuyOrder(buyOrderRequest);
            buyOrderResponseExpected.BuyOrderID = buyOrderResponseFromCreate.BuyOrderID;

            //Assert
            buyOrderResponseFromCreate.BuyOrderID.Should().NotBe(Guid.Empty);
            buyOrderResponseFromCreate.Should().Be(buyOrderResponseExpected);
        }


        #endregion




        #region CreateSellOrder


        [Fact]
        public async Task CreateSellOrder_NullSellOrder_ToBeArgumentNullException()
        {
            //Arrange
            //SellOrderRequest? sellOrderRequest = null;

            //Act
            //await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            //{
            //    await _stocksService.CreateSellOrder(sellOrderRequest);
            //});

            //Arrange
            SellOrderRequest? sellOrderRequest = null;

            //Act
            Func<Task> action = async () =>
            {
                await _stocksSellOrderService.CreateSellOrder(sellOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }



        [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
        [InlineData(0)] //passing parameters to the tet method
        public async Task CreateSellOrder_QuantityIsLessThanMinimum_ToBeArgumentException(uint sellOrderQuantity)
        {
            //Arrange
            //SellOrderRequest? sellOrderRequest = new SellOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = 1, Quantity = sellOrderQuantity };

            //Act
            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    await _stocksService.CreateSellOrder(sellOrderRequest);
            //});
            //Arrange
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
             .With(temp => temp.Quantity, sellOrderQuantity)
             .Create();

            //Act
            Func<Task> action = async () =>
            {
                await _stocksSellOrderService.CreateSellOrder(sellOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }


        [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
        [InlineData(100001)] //passing parameters to the tet method
        public async Task CreateSellOrder_QuantityIsGreaterThanMaximum_ToBeArgumentException(uint sellOrderQuantity)
        {
            //Arrange
            //SellOrderRequest? sellOrderRequest = new SellOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = 1, Quantity = sellOrderQuantity };

            //Act
            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    await _stocksService.CreateSellOrder(sellOrderRequest);
            //});

            //Arrange
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
             .With(temp => temp.Quantity, sellOrderQuantity)
             .Create();

            //Act
            Func<Task> action = async () =>
            {
                await _stocksSellOrderService.CreateSellOrder(sellOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }


        [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
        [InlineData(0)] //passing parameters to the tet method
        public async Task CreateSellOrder_PriceIsLessThanMinimum_ToBeArgumentException(uint sellOrderPrice)
        {
            //Arrange
            //SellOrderRequest? sellOrderRequest = new SellOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = sellOrderPrice, Quantity = 1 };

            //Act
            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    await _stocksService.CreateSellOrder(sellOrderRequest);
            //});
            //Arrange
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
             .With(temp => temp.Price, sellOrderPrice)
             .Create();

            //Act
            Func<Task> action = async () =>
            {
                await _stocksSellOrderService.CreateSellOrder(sellOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }


        [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
        [InlineData(10001)] //passing parameters to the tet method
        public async Task CreateSellOrder_PriceIsGreaterThanMaximum_ToBeArgumentException(uint sellOrderPrice)
        {
            //Arrange
            //SellOrderRequest? sellOrderRequest = new SellOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = sellOrderPrice, Quantity = 1 };

            //Act
            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    await _stocksService.CreateSellOrder(sellOrderRequest);
            //});
            //Arrange
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
             .With(temp => temp.Price, sellOrderPrice)
             .Create();

            //Act
            Func<Task> action = async () =>
            {
                await _stocksSellOrderService.CreateSellOrder(sellOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();

        }


        [Fact]
        public async Task CreateSellOrder_StockSymbolIsNull_ToBeArgumentException()
        {
            //Arrange
            //SellOrderRequest? sellOrderRequest = new SellOrderRequest() { StockSymbol = null, Price = 1, Quantity = 1 };

            //Act
            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    await _stocksService.CreateSellOrder(sellOrderRequest);
            //});
            //Arrange
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
             .With(temp => temp.StockSymbol, null as string)
             .Create();

            //Act
            Func<Task> action = async () =>
            {
                await _stocksSellOrderService.CreateSellOrder(sellOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }


        [Fact]
        public async Task CreateSellOrder_DateOfOrderIsLessThanYear2000_ToBeArgumentException()
        {
            //Arrange
            //SellOrderRequest? sellOrderRequest = new SellOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", DateAndTimeOfOrder = Convert.ToDateTime("1999-12-31"), Price = 1, Quantity = 1 };

            //Act
            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    await _stocksService.CreateSellOrder(sellOrderRequest);
            //});
            //Arrange
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
             .With(temp => temp.DateAndTimeOfOrder, Convert.ToDateTime("1999-12-31"))
             .Create();

            //Act
            Func<Task> action = async () =>
            {
                await _stocksSellOrderService.CreateSellOrder(sellOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();

        }


        [Fact]
        public async Task CreateSellOrder_ValidData_ToBeSuccessful()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
             .Create();


            //Mock
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            _stocksRepositoryMock.Setup(temp => temp.AddSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            //Act
            SellOrderResponse sellOrderResponseFromCreate = await _stocksSellOrderService.CreateSellOrder(sellOrderRequest);

            //Assert
            sellOrder.SellOrderID = sellOrderResponseFromCreate.SellOrderID;
            SellOrderResponse sellOrderResponse_expected = sellOrder.ToSellOrderResponse();
            sellOrderResponseFromCreate.SellOrderID.Should().NotBe(Guid.Empty);
            sellOrderResponseFromCreate.Should().Be(sellOrderResponse_expected);


        }


        #endregion




        #region GetBuyOrders

        //The GetAllBuyOrders() should return an empty list by default
        [Fact]
        public async Task GetAllBuyOrders_DefaultList_ToBeEmpty()
        {
            //Arrange
            List<BuyOrder> buyOrders = new List<BuyOrder>();

            //Mock
            _stocksRepositoryMock.Setup(temp => temp.GetAllBuyOrders()).ReturnsAsync(buyOrders);

            //Act
            List<BuyOrderResponse> buyOrdersFromGet = await _stocksBuyOrderService.GetBuyOrders();

            //Assert
            Assert.Empty(buyOrdersFromGet);
        }


        [Fact]
        public async Task GetAllBuyOrders_WithFewBuyOrders_ToBeSuccessful()
        {
            //Arrange
            List<BuyOrder> buyOrder_requests = new List<BuyOrder>() {
                _fixture.Build<BuyOrder>().Create(),
                _fixture.Build<BuyOrder>().Create()
             };

            List<BuyOrderResponse> buyOrders_list_expected = buyOrder_requests.Select(temp => temp.ToBuyOrderResponse()).ToList();

            //Mock
            _stocksRepositoryMock.Setup(temp => temp.GetAllBuyOrders()).ReturnsAsync(buyOrder_requests);

            //Act
            List<BuyOrderResponse> buyOrders_list_from_get = await _stocksBuyOrderService.GetBuyOrders();


            //Assert
            buyOrders_list_from_get.Should().BeEquivalentTo(buyOrders_list_expected);

        }

        #endregion




        #region GetSellOrders

        //The GetAllSellOrders() should return an empty list by default
        [Fact]
        public async Task GetAllSellOrders_DefaultList_ToBeEmpty()
        {
            //Arrange
            List<SellOrder> sellOrders = new List<SellOrder>();

            //Mock
            _stocksRepositoryMock.Setup(temp => temp.GetAllSellOrders()).ReturnsAsync(sellOrders);

            //Act
            List<SellOrderResponse> sellOrdersFromGet = await _stocksSellOrderService.GetSellOrders();

            //Assert
            Assert.Empty(sellOrdersFromGet);
        }


        [Fact]
        public async Task GetAllSellOrders_WithFewSellOrders_ToBeSuccessful()
        {
            //Arrange
            List<SellOrder> sellOrder_requests = new List<SellOrder>() {
                _fixture.Build<SellOrder>().Create(),
                _fixture.Build<SellOrder>().Create()
            };

            List<SellOrderResponse> sellOrders_list_expected = sellOrder_requests.Select(temp => temp.ToSellOrderResponse()).ToList();
            List<SellOrderResponse> sellOrder_response_list_from_add = new List<SellOrderResponse>();

            //Mock
            _stocksRepositoryMock.Setup(temp => temp.GetAllSellOrders()).ReturnsAsync(sellOrder_requests);

            //Act
            List<SellOrderResponse> sellOrders_list_from_get = await _stocksSellOrderService.GetSellOrders();


            //Assert
            sellOrders_list_from_get.Should().BeEquivalentTo(sellOrders_list_expected);
        }

        #endregion

    }
}


