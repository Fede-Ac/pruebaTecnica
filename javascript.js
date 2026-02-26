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
    // Cargar último pedido
    cargarUltimoPedido(clienteId);
});

function cargarUltimoPedido(clienteId) {

    fetch(`https://localhost:7038/api/Pedidos/ultimo-pedido/${clienteId}`)
        .then(res => {
            if (!res.ok) {
                throw new Error("No tiene pedidos");
            }
            return res.json();
        })
        .then(pedido => {
            detallePedido.innerHTML = `
                <p><strong>ID:</strong> ${pedido.id}</p>
                <p><strong>Fecha:</strong> ${new Date(pedido.fecha).toLocaleDateString()}</p>
                <p><strong>Total:</strong> $${pedido.total}</p>
            `;
        })
        .catch(() => {
            detallePedido.innerHTML = `
                <p>No tiene pedidos registrados.</p>
            `;
        });
}
