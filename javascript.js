const clientesSelect = document.getElementById("clientesSelect");
const pedidosSelect = document.getElementById("pedidosSelect");
const detallePedido = document.getElementById("detallePedido");

// Cargar clientes al iniciar
fetch("https://localhost:7038/api/Pedidos/clientes")
    .then(res => res.json())
    .then(data => {
        data.forEach(cliente => {
            const option = document.createElement("option");
            option.value = cliente.id;
            option.text = cliente.nombre;
            clientesSelect.appendChild(option);
        });
    });

// onchange del select de clientes
clientesSelect.addEventListener("change", function () {

    const clienteId = this.value;

    pedidosSelect.innerHTML = '<option value="">Seleccione un pedido</option>';
    detallePedido.innerHTML = '';

    if (!clienteId) return;

    fetch(`https://localhost:7038/api/Pedidos/pedidos/${clienteId}`)
        .then(res => res.json())
        .then(data => {
            data.forEach(pedido => {
                const option = document.createElement("option");
                option.value = pedido.id;
                option.text = `Pedido ${pedido.id} - $${pedido.total}`;
                pedidosSelect.appendChild(option);
            });
        });
});