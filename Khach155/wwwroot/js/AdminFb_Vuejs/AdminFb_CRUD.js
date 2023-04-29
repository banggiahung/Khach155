Admin_fb = new Vue({
    el: '#Admin_fb',
    data: {
        NguoiBan: null,
        GiaCa: null,

    },
    methods: {

        async AddNewItems() {
            const { value: formValues } = await Swal.fire({
                title: 'Thêm mới sản phẩm',
                html:
                    '<label> Nhập tên người bán </label>' +
                    '<input v-model="NguoiBan" name="NguoiBan" id="NguoiBan" class="swal2-input mb-3" placeholder="Nhập tên">' +
                    '<label> Nhập giá bán</label>' +
                    '<input v-model="GiaCa" name="GiaCa" id="GiaCa" class="swal2-input" type="number" placeholder="Nhập giá">',
                focusConfirm: false,
                allowOutsideClick: false,

                preConfirm: () => {
                    return [
                        document.getElementById('NguoiBan').value,
                        document.getElementById('GiaCa').value
                    ]
                }
            })
            if (formValues) {
                const [NguoiBan, GiaCa] = formValues;
                currentThis = this;
                const formData = new FormData();
                formData.append('Id', 0);
                formData.append('NguoiBan', NguoiBan);
                formData.append('GiaCa', GiaCa);
                formData.append('Cancel', false);
                formData.append('UserId', 1);
                axios.post('/AdminFb/Index', formData, {
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded'
                    }
                }).then(response => {
                    Swal.fire({
                        title: 'Đang thêm mới...',
                        allowOutsideClick: false,
                        onBeforeOpen: () => {
                            Swal.showLoading();
                        },
                        showConfirmButton: false
                    });
                })
                    .then(response => {
                    Swal.fire({
                        icon: 'success',
                        title: 'Thành công',
                        text: 'Đã gửi thành công',
                        confirmButtonText: 'OK',

                    }).then((result) => {
                        if (result.isConfirmed) {
                            window.location.reload();
                        }
                    });
                }).catch((error) => {
                    Swal.fire({
                        icon: 'error',
                        title: 'Lỗi',
                        text: 'Đã có lỗi xảy ra vui lòng thử lại',
                        confirmButtonText: 'OK'
                    });
                    console.error(error);
                });
            }
            
           
           

        }

    }
})