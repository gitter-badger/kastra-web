(function(){

    let path = document.querySelector('#sidebar-wave path');
    let sidebarLinks = document.querySelectorAll('.sidebar a');
    let from = path.getAttribute('d');
    let to = path.getAttribute('data-to');
    let options = {
        type: dynamics.easeOut,
        duration: 300,
        friction: 450
    };
    
    let sidebarOpened = false;
    let button = document.querySelector('#sidebar-menu');

    button.addEventListener('click', function(e) {
        e.stopPropagation();
        e.preventDefault();
        

        if($(button).hasClass('is-opened')) {
            $(button).addClass('is-closed').removeClass('is-opened');
            hideSideBar();
        }
        else {
            $(button).addClass('is-opened').removeClass('is-closed');
            showSidebar();
        }
    });

    document.body.addEventListener('click', function() {

        if(sidebarOpened) {
            $(button).addClass('is-closed').removeClass('is-opened');
            hideSideBar();
        }
    });

    $('#logoutForm a').click(function(){
        $(this).closest('form').submit();
    });

    function showSidebar() {
        document.body.classList.add('has-sidebar');
        sidebarOpened = true;

        dynamics.animate(path, {
            d: to
        }, options);

        sidebarLinks.forEach(function(link, i) {
            dynamics.animate(link, { 
                translateX: 0
            }, Object.assign({}, options, { delay: 50 * i }))
        });
    }

    function hideSideBar() {
        document.body.classList.remove('has-sidebar');

        dynamics.animate(path, {
                d: from
            }, options);

            sidebarLinks.forEach(function(link, i) {
            dynamics.animate(link, { 
                translateX: 200
            }, Object.assign({}, options))
        });
    }
})();