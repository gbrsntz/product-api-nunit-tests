const form = document.getElementById("productForm");
const formTitle = document.getElementById("formTitle");
const productIdInput = document.getElementById("productId");
const nameInput = document.getElementById("name");
const priceInput = document.getElementById("price");
const messageElement = document.getElementById("message");
const productsTableBody = document.getElementById("productsTableBody");
const cancelButton = document.getElementById("cancelButton");
const reloadButton = document.getElementById("reloadButton");

const currencyFormatter = new Intl.NumberFormat("pt-BR", {
    style: "currency",
    currency: "BRL"
});

async function loadProducts() {
    try {
        const response = await fetch("/products");
        const products = await response.json();
        renderProducts(products);
    } catch {
        showMessage("Nao foi possivel carregar os produtos.", true);
    }
}

function renderProducts(products) {
    if (!products.length) {
        productsTableBody.innerHTML = `
            <tr>
                <td colspan="4" class="empty-state">Nenhum produto cadastrado ate o momento.</td>
            </tr>
        `;
        return;
    }

    productsTableBody.innerHTML = products
        .map(product => `
            <tr>
                <td>${product.id}</td>
                <td>${product.name}</td>
                <td>${currencyFormatter.format(product.price)}</td>
                <td>
                    <div class="row-actions">
                        <button class="edit-button" type="button" data-edit="${product.id}">Editar</button>
                        <button class="delete-button" type="button" data-delete="${product.id}">Excluir</button>
                    </div>
                </td>
            </tr>
        `)
        .join("");
}

function resetForm() {
    form.reset();
    productIdInput.value = "";
    formTitle.textContent = "Novo produto";
}

function showMessage(text, isError = false) {
    messageElement.textContent = text;
    messageElement.className = `message ${isError ? "error" : "success"}`;
}

async function handleSubmit(event) {
    event.preventDefault();

    const payload = {
        name: nameInput.value.trim(),
        price: Number(priceInput.value)
    };

    const id = productIdInput.value;
    const url = id ? `/products/${id}` : "/products";
    const method = id ? "PUT" : "POST";

    try {
        const response = await fetch(url, {
            method,
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(payload)
        });

        if (!response.ok) {
            const error = await response.json().catch(() => ({ message: "Erro ao salvar produto." }));
            showMessage(error.message || "Erro ao salvar produto.", true);
            return;
        }

        showMessage(id ? "Produto atualizado com sucesso." : "Produto cadastrado com sucesso.");
        resetForm();
        await loadProducts();
    } catch {
        showMessage("Nao foi possivel comunicar com a API.", true);
    }
}

async function handleTableClick(event) {
    const editId = event.target.getAttribute("data-edit");
    const deleteId = event.target.getAttribute("data-delete");

    if (editId) {
        try {
            const response = await fetch(`/products/${editId}`);
            if (!response.ok) {
                showMessage("Produto nao encontrado para edicao.", true);
                return;
            }

            const product = await response.json();
            productIdInput.value = product.id;
            nameInput.value = product.name;
            priceInput.value = product.price;
            formTitle.textContent = `Editando produto #${product.id}`;
            showMessage("Produto carregado para edicao.");
        } catch {
            showMessage("Nao foi possivel carregar o produto.", true);
        }
    }

    if (deleteId) {
        try {
            const response = await fetch(`/products/${deleteId}`, { method: "DELETE" });
            if (!response.ok) {
                showMessage("Produto nao encontrado para exclusao.", true);
                return;
            }

            showMessage("Produto removido com sucesso.");
            if (productIdInput.value === deleteId) {
                resetForm();
            }

            await loadProducts();
        } catch {
            showMessage("Nao foi possivel excluir o produto.", true);
        }
    }
}

form.addEventListener("submit", handleSubmit);
productsTableBody.addEventListener("click", handleTableClick);
cancelButton.addEventListener("click", () => {
    resetForm();
    showMessage("Edicao cancelada.");
});
reloadButton.addEventListener("click", loadProducts);

loadProducts();
