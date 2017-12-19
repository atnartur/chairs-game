import $ from 'jquery';
import MK from 'matreshka';
import Model from "./Model";

export default class Chairs extends MK.Array {
    get Model() {
        return Model;
    }
    get itemRenderer() {
        return '<img src="images/chair.png" class="chair">';
    }
    constructor() {
        super();
        this
            .set({
                isHide: true
            })
            .bindNode({
                sandbox: '#chairs',
                container: ":sandbox"
            })
            .bindNode('isHide', ':sandbox', MK.binders.display(false));
    }
    show() {
        this.isHide = false;
    }
    setPositions(count) {
        let $elem = $(this.$nodes.sandbox);
        console.log($elem);
        let w = $elem.width();
        let h = $elem.height();
        let xStart = w / 2;
        let yStart = h / 2;

        let chairs = [];

        this.widthCenter = Math.round(w / 2);
        this.heightCenter = Math.round(h / 2);
        console.log(w,  h,  this.widthCenter, this.heightCenter);
        let r = 0;
        
        if (count !== 1)
            r = w / Math.round(2*Math.sin(Math.PI / count)) + this.heightCenter / 2;

        for (let i = count-1; i >= 0; i--) {
            let a = i * (2 * Math.PI / count);
            let y = Math.round((Math.cos(a) * r + yStart));
            let x = Math.round((Math.sin(a) * r + xStart)) - this.widthCenter;
            
            chairs.push({
                rotate: (360 / count) * i, 
                x, 
                y
            });
        }
        console.log(chairs);
        this.recreate(chairs);
    }
}