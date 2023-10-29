$.ajax({
    url: "/api/CheckLoginStatusAPI",
    method: "GET",
    success: function (response) {
        if (response.status == "login") {
            $("#navContainer").append(`
            <div class="navbar-collapse collapse d-sm-inline-flex justify-content-between" id="navLst">
                <ul class="navbar-nav flex-grow-1">

                    <li class="nav-item">
                        <a class="nav-link text-dark" href="/Product/ProductList">Products</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-dark" href="/Order/OrderList">Orders</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-dark" href="/Customer/CustomerList">Customers</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-dark" href="/Supplier/SupplierList">Suppliers</a>
                    </li>                  
                    <li class="nav-item" id="li-logout">
                        <a class="nav-link text-dark" href="/Home/Logout">Log out</a>
                    </li>
                </ul>
            </div>
            `);

            if (response.permission == 0) {
                var newNavItem = `
                    <li class="nav-item">
                        <a class="nav-link text-dark" href="/User/UserList">ERP Users</a>
                    </li>
                `;
                $(newNavItem).insertBefore("#li-logout");
            }
        }
    },
    error: function (e) {
        $("#navLst").remove();
        console.log("Failed in API", e);
    }
})