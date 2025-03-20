#pragma once

#include <cmath>
#include <cstdint>

#include "cuda_runtime.h"
#include "device_launch_parameters.h"

#include "Lamp.hpp"
#include "Ray.hpp"
#include "LightRay.hpp"
#include "RaySection.hpp"
#include "PairDouble.hpp"

cudaError_t GetInitialIntensity(Lamp lamp, double* res, int lng, int ltd);