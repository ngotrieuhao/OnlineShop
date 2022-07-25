var OrderController = function () {
    this.initialize = function () {
        registerEvent();
    }

    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger'
        },
        buttonsStyling: false
    })

    function registerEvent() {
        // Confirm order
        $('body').on('click', '.btn-update-status', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            updateOrder(id);
        });

        // Cancel order button
        $('body').on('click', '.btn-cancel', function (e) {
            swalWithBootstrapButtons.fire({
                title: 'Are you sure you want to cancel your order?',
                text: "You will not be able to restore your order!",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText:'Confirm',
                cancelButtonText: 'Cancel',
                reverseButtons: false
            }).then((result) => {
                if (result.isConfirmed) {
                    e.preventDefault();
                    const id = $(this).data('id');
                    cancelOrder(id);
                } else if (
                    /* Read more about handling dismissals below */
                    result.dismiss === Swal.DismissReason.cancel
                ) {
                    swalWithBootstrapButtons.fire(
                        'Cancel',
                        'No more canceling orders',
                        'error'
                    )
                }
            })
            
        });
    }

    function cancelOrder(id) {
        $.ajax({
            type: 'POST',
            url: '/Order/CancelOrderStatus',
            data: {
                orderId: id
            },
            success: function (res) {
                localStorage.setItem("canceledOrder", true);
                location.reload();
            },
            error: function (err) {
                console.log(err);
            }
        });
    }

    function updateOrder(id) {
        $.ajax({
            type: 'POST',
            url: '/Order/UpdateOrderStatus',
            data: {
                orderId: id
            },
            success: function (res) {
                localStorage.setItem("updatedOrder", true);
                location.reload();
            },
            error: function (err) {
                console.log(err);
            }
        });
    }

    $(document).ready(function () {
        // This function will run on every page reload, but the alert will only 
        // happen on if the buttonClicked variable in localStorage == true
        if (localStorage.getItem("canceledOrder")) {
            localStorage.removeItem("canceledOrder");
            swalWithBootstrapButtons.fire(
                'Cancel order successfully!',
                'Order has been cancelled',
                'success'
            )
        } else if (localStorage.getItem("updatedOrder")) {
            localStorage.removeItem("updatedOrder");
            Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'Order status update successful',
                showConfirmButton: false,
                timer: 1500,
            })
        }
    });
}


