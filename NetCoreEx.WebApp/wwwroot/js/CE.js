(function ($) {
    jQuery.fn.CE = function () {
        var args = Array.prototype.slice.call(arguments);

        if (args.length === 1 && typeof (args[0]) == "object") {
            build.call(this, args[0]);
        }
    };

    function build(options) {
        this.each(function () {
            createCe(this, options.maxHeight);
        });
    }

    function createCe(elementCe, maxHeight) {
        if ($(elementCe).height() >= maxHeight + 12) {
            $(elementCe).wrap("<div class='ContentReadMore'></div>");
            $(elementCe).parent().wrap("<div class='divContentReadMore'></div>");
            var divContentReadMore = $(elementCe).parent().parent();
            divContentReadMore.append("<a class='aReadMore' ><img src='https://cdn.phudev.com/images/expanded.png'/></a>");

            if (maxHeight === 0)
                maxHeight = parseInt(divContentReadMore.css("max-height"));

            divContentReadMore.css("max-height", maxHeight);
            divContentReadMore.find(".aReadMore").css('display', 'block');
            divContentReadMore.find(".aReadMore").css('position', 'absolute');
            divContentReadMore.find(".aReadMore").attr("title", "");
            var color = divContentReadMore.parent().css("background-color");

            if ($.trim(color) === "rgba(0, 0, 0, 0)")
                color = "White";
            divContentReadMore.find(".aReadMore").css("background", "linear-gradient(#ffffff36," + color + ")");
            createTooltip(divContentReadMore.find(".aReadMore"), divContentReadMore.find(".ContentReadMore").html());

            divContentReadMore.children().each(function () {
                if (!$(this).hasClass("aReadMore")) {
                    $(this).css('line-height', '20px');
                    $(this).children().each(function () {
                        if (!$(this).hasClass("aReadMore")) {
                            $(this).css('line-height', '20px');
                        }
                    });
                }
            });
            var aReadMore = $(elementCe).parent().parent().children(".aReadMore");

            aReadMore.click(function (e) {
                var td = divContentReadMore.parent("td");
                if (td != null)
                    td.parent("tr").children().each(function () {
                        var divContentReadMore = $(this).find(".divContentReadMore");
                        if (divContentReadMore != null && divContentReadMore != $(this))
                            readMoreClick(divContentReadMore, maxHeight);
                    });
                else
                    readMoreClick($(this), maxHeight);
            });
            divContentReadMore.click(function (e) {
                if (e.ctrlKey) {
                    var divContentReadMoreParent = $(this).parent("td").find(".divContentReadMore");
                    var coll = false;
                    if (divContentReadMoreParent != null && divContentReadMoreParent !== $(this)) {
                        var content = divContentReadMoreParent.find("img").attr('src');
                        if (divContentReadMoreParent.find("img").attr("src") === "https://cdn.phudev.com/images/expanded.png") {
                            coll = true;
                        }
                    }
                    $(this).parent("td").parent().parent().children().each(function () {
                        var divContentReadMore = $(this).find(".divContentReadMore");
                        if (divContentReadMore != null && divContentReadMore !== $(this) && checkCollapsed(divContentReadMore) == coll)
                            readMoreClick(divContentReadMore, maxHeight);
                    });
                }
            });
        }
    }

    function readMoreClick(e, maxHeight) {
        if (e.parent().find("img").attr("src") === "https://cdn.phudev.com/images/expanded.png") {
            e.css('max-height', 'none');
            e.css('height', 'auto');
            e.parent().find(".aReadMore img").attr("src", "https://cdn.phudev.com/images/collapsed.png");
            e.find(".aReadMore").css("background", "none");
            createTooltip(e.parent().find(".aReadMore"), "", 0, 0);
        }
        else {
            e.css('max-height', maxHeight);
            e.css('height', "none");
            e.parent().find(".aReadMore img").attr("src", "https://cdn.phudev.com/images/expanded.png");
            var color = e.parent().css("background-color");

            if ($.trim(color) == "rgba(0, 0, 0, 0)")
                color = "White";
            e.find(".aReadMore").css("background", "linear-gradient(#ffffff36," + color + ")");
            var content = e.find(".ContentReadMore").html();
            createTooltip(e.parent().find(".aReadMore"), content);
        }
    }

    function createTooltip(control, contentTooltip) {
        $(control).tooltip({
            content: contentTooltip,
            track: true,
            open: function (event, ui) {
                ui.tooltip.css("max-width", "600px");
                ui.tooltip.css("position", "fixed");
                ui.tooltip.css("white-space", "normal");
            }
        });
    }

    function checkCollapsed(e) {
        if (e.parent().find("img").attr("src") === "https://cdn.phudev.com/images/expanded.png") {
            return true;
        }
        else {
            return false;
        }
    }
})(jQuery);