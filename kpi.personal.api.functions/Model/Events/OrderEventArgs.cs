using System;

namespace kpi.personal.api.functions.Model.Events
{
    public class OrderEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Número do pedido
        /// </summary>
        public int OrderNumber { get; set; }

        /// <summary>
        /// Código da pessoa
        /// </summary>
        public int RepresentativeCode { get; set; }

        /// <summary>
        /// Pontos
        /// </summary>
        public int? PointsQuantity { get; set; }

        /// <summary>
        /// Valor líquido
        /// </summary>
        public decimal? NetValue { get; set; }

        /// <summary>
        /// Campanha aprovação
        /// </summary>
        public int? ApprovalCycle { get; set; }

        /// <summary>
        /// Campanha de cancelamento
        /// </summary>
        public int? CancellationCycle { get; set; }
    }
}
