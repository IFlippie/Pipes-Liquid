void InitializeFragmentNormal(inout Interpolators i) {
    i.normal = normalize(cross(ddy(worldPosition), ddx(worldPosition)))
}