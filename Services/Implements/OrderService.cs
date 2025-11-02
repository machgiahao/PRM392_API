using AutoMapper;
using Repositories.Entities;
using Repositories.Repositories.Interfaces;
using Repositories.Uow;
using Services.Dtos;
using Services.Interfaces;

namespace Services.Implements
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _uow;
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork uow, IOrderRepository orderRepository, ICartRepository cartRepository, IMapper mapper)
        {
            _uow = uow;
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<Order> CreateOrderFromCartAsync(int userId, string paymentMethod, string billingAddress, CancellationToken cancellationToken = default)
        {
            var activeCart = await _cartRepository.GetOrCreateActiveCartByUserIdAsync(userId);

            if (activeCart == null || !activeCart.CartItems.Any())
            {
                throw new InvalidOperationException("Cart is emmpty. Can not create order.");
            }

            var newOrder = new Order
            {
                UserId = userId,
                CartId = activeCart.CartId, 
                PaymentMethod = paymentMethod,
                BillingAddress = billingAddress,
                OrderStatus = "Pending", 
                OrderDate = DateTime.UtcNow
            };
            await _orderRepository.AddAsync(newOrder, cancellationToken);

            activeCart.Status = "Completed";
            _cartRepository.UpdateCart(activeCart);

            await _uow.SaveChangesAsync(cancellationToken);
            return newOrder;
        }

        public async Task<IEnumerable<OrderDto>> GetUserOrdersAsync(int userId, CancellationToken cancellationToken = default)
        {
            var listOrder = await _orderRepository.GetOrdersByUserIdAsync(userId, cancellationToken);
            return _mapper.Map<IEnumerable<OrderDto>>(listOrder);
        }

        public async Task<OrderDetailDto?> GetOrderDetailsAsync(int orderId, CancellationToken cancellationToken = default)
        {
            var orderEntity = await _orderRepository.GetOrderDetailAsync(orderId, cancellationToken);

            if (orderEntity == null)
            {
                return null; 
            }
            
            return _mapper.Map<OrderDetailDto>(orderEntity);
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, string newStatus, CancellationToken cancellationToken = default)
        {
            var order = await _orderRepository.GetByIdAsync<int>(orderId, cancellationToken);

            if (order == null)
            {
                return false; 
            }

            order.OrderStatus = newStatus;

            _orderRepository.Update(order, cancellationToken);

            await _uow.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
