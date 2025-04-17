document.addEventListener('DOMContentLoaded', function () {
    // Dropdown tự đóng khi bấm ra ngoài
    const dropdown = document.getElementById('userDropdown');
    const dropdownMenu = document.querySelector('.dropdown-menu');

    document.addEventListener('click', function (event) {
        const isClickInside = dropdown.contains(event.target) || dropdownMenu.contains(event.target);

        if (!isClickInside) {
            const dropdownInstance = bootstrap.Dropdown.getInstance(dropdown);
            if (dropdownInstance) {
                dropdownInstance.hide();
            }
        }
    });
});
