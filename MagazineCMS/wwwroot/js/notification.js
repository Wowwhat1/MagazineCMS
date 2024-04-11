countNotification();

function countNotification() {
    var url = '/notification/getall';
    $.getJSON(url, function (response) {
        var unreadCount = 0;
        if (response != null) {
            var notifications = response.data;
            notifications.forEach(notification => {
                // Check if notification is unread and increment the counter
                if (!notification.isRead) {
                    unreadCount++;
                }
            });

            document.querySelector('#notification-count').innerHTML = unreadCount == 0 ? "" : unreadCount + "+";
        }
    })

}


function showNotification() {
    var actionUrl = '/notification/getfewnotification';
    $.getJSON(actionUrl, function (response) {
        document.querySelector('#notification-content').innerHTML = '';
        if (response != null) {
            var notifications = response.data;
            notifications.forEach(notification =>  {
                const notificationElement = document.createElement('div');
                const createdAt = new Date(notification.createdAt);
                const currentTime = new Date();
                const timeDiff = Math.abs(currentTime - createdAt);
                let displayTime;

                if (timeDiff < 60000) {
                    displayTime = 'few minutes ago';
                } else if (timeDiff < 3600000) {
                    const minutes = Math.floor(timeDiff / 60000);
                    displayTime = minutes + ' minute' + (minutes > 1 ? 's' : '') + ' ago';
                } else if (timeDiff < 86400000) {
                    const hours = Math.floor(timeDiff / 3600000);
                    displayTime = hours + ' hour' + (hours > 1 ? 's' : '') + ' ago';
                } else {
                    const days = Math.floor(timeDiff / 86400000);
                    displayTime = days + ' day' + (days > 1 ? 's' : '') + ' ago';
                }

                var iconClass = notification.type == "Submit" ? "fa fa-file-text" : notification.Type == "Approval" ? "fa fa-check" : notification.Type == "Reject" ? "fa fa-minus" : "fa fa-info";
                var color = notification.type == "Submit" ? "primary" : notification.Type == "Approval" ? "success" : notification.Type == "Reject" ? "danger" : "primary";

                notificationElement.innerHTML = `
                <a class="dropdown-item d-flex align-items-center" href="${notification.url}" onClick="markAsRead(${notification.id})">
                    <div class="mr-3">
                        <div class="icon-circle bg-${color}">
                            <i class="fa ${iconClass} text-white"></i>
                        </div>
                    </div>
                    <div>
                        <div class="small text-gray-500">${displayTime}</div>
                        <span class="font-weight-bold">${notification.senderUserName}</span>
                        <span class="">${notification.content}</span>
                    </div>
                    ${!notification.isRead?
                    `<div>
                        <i class="fa fa-circle text-primary"></i>
                    </div>` : ''}
                </a>
            `;
                document.querySelector('#notification-content').appendChild(notificationElement);
            });

        } else {
            document.querySelector('#notification-content').innerHTML = '';
        }
    });
}

function markAsRead(id) {
    var actionUrl = '/notification/markasread/' + id;
    $.post(actionUrl, function (response) {
        if (response.success) {
            countNotification();
        }
    });
}