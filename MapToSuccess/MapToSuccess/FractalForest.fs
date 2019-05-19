module MapToSuccess.FractalForest

open System
open System.Drawing
open System.Windows.Forms

// Create a form to display the graphics
let width, height = 1000, 850
let form = new Form(Width = width, Height = height)
let box = new PictureBox(BackColor = Color.White, Dock = DockStyle.Fill)
let image = new Bitmap(width, height)
let graphics = Graphics.FromImage(image)
graphics.SmoothingMode <- System.Drawing.Drawing2D.SmoothingMode.HighQuality
let brush = new SolidBrush(Color.FromArgb(0, 0, 0))

box.Image <- image
form.Controls.Add(box)

// Compute the endpoint of a line
// starting at x, y, going at a certain angle
// for a certain length. 
let endpoint x y angle length =
    x + length * cos angle,
    y + length * sin angle

let flip x = (float)height - x

// Utility function: draw a line of given width, 
// starting from x, y
// going at a certain angle, for a certain length.
let drawLine (target : Graphics) (brush : Brush) 
             (x : float) (y : float) 
             (angle : float) (length : float) (width : float) =
    let x_end, y_end = endpoint x y angle length
    let origin = new PointF((single)x, (single)(y |> flip))
    let destination = new PointF((single)x_end, (single)(y_end |> flip))
    let pen = new Pen(brush, (single)width)
    target.DrawLine(pen, origin, destination)

let draw x y angle length width = 
    drawLine graphics brush x y angle length width

let pi = Math.PI

//Many thanks to Dmitriy Kakhovsky for his contributions, insights, and gentle prodding to get this looking nicer.

//Special notes:
//  1)  We have a progressive greening, where each level gets slightly more green.
//  2)  The next branch's angle changes by a random number between 0 and 0.4.
//  3)  We call drawTree in another recursive function and layer tree after tree on top to give depth.
//  4)  In drawBigTree, we shift x slightly to make the trunk a little more realistic and spread out the branches a bit.
let rand = System.Random()

let rec drawOnScreen x y rad (currentlevel : float) maxlevels =
    let len = (maxlevels - currentlevel - 1.0) * 9.0
    let newg = Convert.ToInt32(currentlevel) * 10
    let newbrush = new SolidBrush(Color.FromArgb(0, newg, 0))
    drawLine graphics newbrush x y (pi*rad) len (maxlevels - currentlevel)
    let newx, newy = endpoint x y (pi*(rad)) len
    if currentlevel < maxlevels then
        drawOnScreen newx newy (rad + (rand.NextDouble() * 0.4))  (currentlevel + 1.0) maxlevels
        drawOnScreen newx newy (rad - (rand.NextDouble() * 0.4))  (currentlevel + 1.0) maxlevels

let drawTree startx starty (maxlevels : float) =
    draw startx starty (pi*(0.5)) (maxlevels * 9.0) maxlevels
    let x, y = endpoint startx starty (pi*(0.5)) (maxlevels * 9.0)
    let len = (maxlevels - 1.0) * 9.0
    drawOnScreen x y 0.6 1.0 maxlevels
    drawOnScreen x y 0.4 1.0 maxlevels

let rec drawBigTree x y len =
    if len > 0. then
        drawTree x y len
        drawBigTree (x - 2.) y (len - 1.)

let displayTree x y len =
    drawBigTree x y len
    form.ShowDialog()
//drawBigTree 500. 50. 14.