# Stereo and Parameter Tuner for Tango
## Usage
  * See ParameterTunerTestScene1 as an example
  * To use in other projects
    1. Export Unity Package with the following folders
      * ParameterTuner
      * Standard Assets
      * StereoCamera
    2. Replace Tango camera with TangoPhoneSpace
    3. Add CalibrationCanvas

## Key Mappings
  * w/s Select parameters
  * a/d Tune selected parameter
  * q/e Adjust delta

## UDP Mode
* Run the following command in Bash. Replace IP with your device IP.

        while read -n 1 x; do echo $x > /dev/udp/192.168.0.22/5555; done

